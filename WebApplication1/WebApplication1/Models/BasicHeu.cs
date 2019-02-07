using System;
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
        public delegate void viewEventHandler(object sender, string textToWrite);
        public event viewEventHandler FlushText;

        int n, m;
        GAPinstance GAP;

        public BasicHeu(GAPinstance gap)
        {
            GAP = gap;
            n = GAP.numCli;
            m = GAP.numServ;
        }

        public int constructFirtsSol()
        {

            //si ordinano i clienti per regret decrescenti e poi li si assegna a turno, 
            //ordinando per ciascuno i magazzini per richieste crescenti

            GAP.sol = new int[n];

            int i, j, ii,z;

            int[] capleft = new int[n];
            for (i = 0; i < m; i++) capleft[i] = GAP.cap[i];


            int[] keys = new int[m];
            int[] ind = new int[m];

            for (j = 0; j < n; j++)
            {
                for (i = 0; i < m; i++)
                {
                    keys[i] = GAP.req[i, j];
                    ind[i] = i;
                }

                Array.Sort(keys, ind); // ordina in base alle richieste crescenti

                for (ii = 0; ii < m; ii++)
                {
                    i = ind[ii];
                    if (capleft[i] >= GAP.req[i, j])
                    {
                        GAP.sol[j] = i; //ad ogni cliente viene associato un magazzino
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
            z = checkSol(GAP.sol);

            return z;
        }

        public int checkSol(int[] sol)
        {
            int z = 0, j, i;
            int[] capused = new int[n];
            for (i = 0; i < m; i++) capused[i] = 0;

            // controllo assegnamenti
            for (j = 0; j < n; j++)
                if (sol[j] < 0 || sol[j] >= m)
                {
                    z = int.MaxValue;
                    return z;
                }
                else
                {
                    z += GAP.cost[sol[j], j];
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


        public int opt10(int[,] cost)
        {
            /*Si considera a turno ogni client e si prova a riassegnarlo ad ogni altro magazzino
            che ha capacità residua sufficiente.*/

            int z = 0;
            int i, j = 0;
            bool isImproved;

            int[] capleft = new int[m];
            for (i = 0; i < m; i++)
            {
                capleft[i] = GAP.cap[i]; //assegno capacità
            }

            for(j = 0; j< n; j++)
            {
                capleft[GAP.sol[j]] -= GAP.req[GAP.sol[j], j]; //riduco le capacità
                z += GAP.cost[GAP.sol[j], j]; //soluzione: costo del server per il client
            }

            do
            {
                isImproved = false;
                for (j = 0; j < n; j++)
                {         
                    for (i = 0; i < m; i++)
                    {
                        if (cost[i,j] < cost[GAP.sol[j],j] && capleft[i] >= GAP.req[i,j]){ //costo attuale < costo e ci sono capacità rimanenti

                            z -= cost[GAP.sol[j], j] - cost[i, j]; //costo iniziale - costo attuale
                            capleft[GAP.sol[j]] += GAP.req[GAP.sol[j], j]; //riduco capacità dei clienti
                            
                            GAP.sol[j] = i; //assegno server
                            capleft[i] -= GAP.req[i,j]; //riduco capacità dei server

                            System.Diagnostics.Debug.WriteLine("opt10, improvement. z=" + z);
                            isImproved = true;
                            break;
                        }
                    }
                }
                if (isImproved) break;
            } while (isImproved);

            int zcheck = checkSol(GAP.sol);
            if(z != zcheck)
            {
                z = int.MaxValue;
            }
            return z;
        }


       public int simulatedAnnealing()
       {
            int j, i, p = 0; 

            int k = 10; //costante di Boltzmann

            int iter = 0;

            double maxTemp = 1000;
            int maxIter = 10;
            double alpha = 0.95; //geometric cooling
            double prob = 0.001;

            double temp = maxTemp;

            int z = constructFirtsSol();

            int[] sol = GAP.sol;
            int[,] cost = GAP.cost;

            int[] capleft = new int[m];
            for (i = 0; i < m; i++) capleft[i] = GAP.cap[i]; //assegno capacità
            
            Random rand = new Random(100);

            bool isFinished = true;
            while (isFinished)
            {

                if (iter < maxIter) temp = maxTemp;
                else isFinished = false;
                
                iter++;

                // generare sol casuale
                j = rand.Next(0, n - 1);    //clienti
                int isol = GAP.sol[j];      
                i = rand.Next(0, m - 1);    //magazzini

                int[] tmpSol = GAP.sol;
                tmpSol[j] = i;
                capleft[i] -= GAP.req[i, j];
                capleft[isol] += GAP.req[isol, j];

                int lastCost = z;
                lastCost -= (GAP.cost[isol, j] - GAP.cost[i, j]);

                int firstCost = z;

                if (lastCost < firstCost)
                {
                    GAP.sol = tmpSol;
                    GAP.cap = capleft;
                    z = lastCost;
                }
                else if (capleft[i] >= GAP.req[i, j])
                {
                    //capacità rimanenti
                    p = (int)Math.Exp(-(firstCost - lastCost) / (k * temp));

                    int rnd = rand.Next(0, 100);
                    if (rnd < p * 100)
                    {
                        GAP.sol = tmpSol;
                        GAP.cap = capleft;
                        z = lastCost;
                    }
                }

                //1000 numero di step decisi da me
                if ((iter % 1000) == 0)
                {
                    temp = alpha * temp;  //decremento temp
                }
            }

            int zcheck = checkSol(GAP.sol);
            if (z != zcheck)
            {
                z = int.MaxValue;
            }

            return z;
        }

    }
}
 