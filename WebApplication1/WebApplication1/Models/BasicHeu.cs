﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using Newtonsoft.Json;
using System.IO;
using System.Data;


namespace WebApplication1.Models
{
    class BasicHeu
    {
        private GAPinstance GAP;
        private int n, m;
        public int[] sol;
        int[] capacitiesLeft;

        public BasicHeu(GAPinstance gap)
        {
            GAP = gap;
            n = GAP.numCli;
            m = GAP.numServ;
            capacitiesLeft = (int[])GAP.cap.Clone();
        }

        public int CheckSol(int[] sol)
        {
            int z = 0, j, i;
            int[] capused = new int[n];
            for (i = 0; i < m; i++) capused[i] = 0;

            // controllo assegnamenti
            for (j = 0; j < n; j++) { 
                if (sol[j] < 0 || sol[j] >= m)
                {
                    z = int.MaxValue;
                    return z;
                }
                else
                {
                    z += GAP.cost[sol[j], j];
                }
            }

            // controllo capacità
            for (j = 0; j < n; j++)
            {
                capused[sol[j]] += GAP.req[sol[j], j];
                if (capused[sol[j]] > GAP.cap[sol[j]])
                {
                    z = int.MaxValue;
                    return z;
                }
            }
            return z;
        }

        public int ConstructiveSolution()
        {

            //si ordinano i clienti per regret decrescenti e poi li si assegna a turno, 
            //ordinando per ciascuno i magazzini per richieste crescenti
            int ii, z;
            sol = new int[n];

            int[] capleft = new int[n];
            for (int i = 0; i < m; i++) capleft[i] = GAP.cap[i];


            int[] keys = new int[m];
            int[] index = new int[m];

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    keys[i] = GAP.req[i, j];
                    index[i] = i;
                }
                Array.Sort(keys, index); // ordina in base alle richieste crescenti
              
                for (ii = 0; ii < m; ii++)
                {
                    int i = index[ii];
                    //se la capactià del server è sufficiente assegno il client a quel server
                    if (capleft[i] >= GAP.req[i, j])
                    {
                        sol[j] = i; //ad ogni cliente viene associato un magazzino
                        capleft[i] -= GAP.req[i, j]; //riduzione capacità rimanenti
                        break;
                    }
                    if (ii == m)
                    {
                        z = 0;
                        goto lend;
                    }
                }
            }

            lend:
            z = CheckSol(sol);
            GAP.zub = z;
            return z;
        }

        public int Opt10(int[,] cost)
        {
            /*Si considera a turno ogni client e si prova a riassegnarlo ad ogni altro magazzino
            che ha capacità residua sufficiente.*/

            int z = 0, isol = 0;
            int[,] req = GAP.req;
            bool isImproved = true;

            int[] capleft = new int[m];
            for (int i = 0; i < m; i++) capleft[i] = GAP.cap[i]; //assegno capacità
            

            for(int j = 0; j < n; j++)
                z += GAP.cost[sol[j], j]; //soluzione: costo del server per il client
            
        
            while (isImproved)
            {
                isImproved = false;
                for (int j = 0; j < n; j++)
                {         
                    for (int i = 0; i < m; i++)
                    {
                        isol = sol[j];
                        if (i != isol && cost[i,j] < cost[isol, j] && capleft[i] >= req[i,j]){ //costo attuale < costo e ci sono capacità rimanenti

                            sol[j] = i; //assegno server
                            capleft[i] -= req[i, j]; //riduco capacità dei server
                            capleft[isol] += req[isol, j]; //riduco capacità dei clienti
                            z -= (cost[isol, j] - cost[i, j]); //costo iniziale - costo attuale

                            System.Diagnostics.Debug.WriteLine("opt10, improvement. z=" + z);
                            isImproved = true;

                            if(z < GAP.zub)
                            {
                                GAP.zub = z;
                            }
                        }
                    }
                }
            }

            double zcheck = 0;
            for (int j = 0; j < n; j++)
            {
                zcheck += cost[sol[j], j];
            }

            if (Math.Abs(z - zcheck) > 0.01)
            {
                return -1;
            }

            return z;
        }

        
       public int SimulatedAnnealing()
       {
            int z = this.ConstructiveSolution();
            sol = SimulatedAnnealingAlgorithm(sol, z);
            return CheckSol(sol);
       }

       public int[] SimulatedAnnealingAlgorithm(int[] currentSol, int currentCost)
       {
            int i, j;
            double p;
            int k = 10; //costante di Boltzmann
            int coolingSchedule = 1000000;
            int maxIter = 100000;
            double alpha = 0.95; //geometric cooling
            double temp = 2;

            int iter = 0;

            int bestCost = currentCost;
            int[] bestSol = new int[currentSol.Length];
            int[] newSol = (int[])currentSol.Clone();

            Random rand = new Random(100);

            do
            {
                iter++;
                i = rand.Next(0, m);
                j = rand.Next(0, n);
                newSol[j] = i;
                int newCost = CheckSol(newSol);

                if(newCost <= currentCost)
                {
                    currentCost = newCost;
                    currentSol = newSol;
                }
                else
                {
                    p = Math.Exp((-(newCost - currentCost)) / (k*temp));
                    double rndProb = rand.NextDouble();
                    if (rndProb < p)
                    {
                        currentCost = newCost;
                        currentSol = newSol;
                    }
                }

                if (currentCost < bestCost)
                {
                    bestCost = currentCost;
                    bestSol = (int[])currentSol.Clone();
                }

                if ((iter % coolingSchedule) == 0)
                {
                    temp *= alpha;
                }

            } while (iter <= maxIter);

            return bestSol;
        }

        public int TabuSearch()
        {
            int z = this.ConstructiveSolution();
            sol = TabuSearchAlgorithm(sol, z);
            return CheckSol(sol);
        }

        public int[] TabuSearchAlgorithm(int[] bestSol, int bestCost) {

            int tabuTenure = 100;
            int maxIter = 10000;
            int iter = 0;

            int[] currentSol = new int[n];
            int[,] tabuList = new int[m, n];

            int currentCost = bestCost;
            //currentSol = (int[])bestSol.Clone();
            Array.Copy(bestSol, currentSol, bestSol.Length);

            do
            {
                iter++;
                int server = 0;
                int client = 0;
                int[] bestLocalSol = new int[currentSol.Length];
                int bestLocalCost = int.MaxValue;

                bool found = false;
                bool admissible = true;

                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < m; i++)
                    {
                        int[] solutionToEvaluate = new int[currentSol.Length];

                        Array.Copy(currentSol, solutionToEvaluate, currentSol.Length);
                        solutionToEvaluate[j] = i;
                        int costSolutionToEvaluate = CheckSol(solutionToEvaluate);

                        // aspiration
                        if (currentCost < bestCost)
                        {
                            bestCost = currentCost;
                            bestSol = (int[])currentSol.Clone();
                        }

                        System.Diagnostics.Debug.WriteLine("i " + i + " currentSol[j] " + currentSol[j]); //DIVERSI
                        System.Diagnostics.Debug.WriteLine((tabuList[i, j] + tabuTenure) + " ------------ " + iter + "-----" + costSolutionToEvaluate);
                        // found solution
                        if (i != currentSol[j] && (tabuList[i, j] + tabuTenure < iter) && costSolutionToEvaluate < int.MaxValue)
                        {
                            System.Diagnostics.Debug.WriteLine("dentro if");
                            if (!found)
                            {
                                System.Diagnostics.Debug.WriteLine("not found");
                                found = true;
                                bestLocalSol = (int[])solutionToEvaluate.Clone();
                                bestLocalCost = costSolutionToEvaluate;
                                server = i;
                                client = j;
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("found");
                                // check if is better in Neighborhood
                                if (costSolutionToEvaluate < bestLocalCost)
                                {
                                    bestLocalSol = (int[])solutionToEvaluate.Clone();
                                    bestLocalCost = costSolutionToEvaluate;
                                    server = i;
                                    client = j;
                                }
                            }
                        }
                        
                    }
                }

                tabuList[server, client] = iter;

                currentSol = (int[])bestLocalSol.Clone();
                currentCost = bestLocalCost;
                if (currentCost < bestCost)
                {
                    bestCost = currentCost;
                    bestSol = (int[])currentSol.Clone();
                }

            } while (iter <= maxIter);

            return bestSol;
        }



    }
}
 