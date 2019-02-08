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

        GAPinstance GAP;
        int n, m;
        public int[] sol;

        public BasicHeu(GAPinstance gap)
        {
            GAP = gap;
            n = GAP.numCli;
            m = GAP.numServ;
        }

        public int checkSol(int[] sol)
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

        public int constructiveSolution()
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
            z = checkSol(sol);
            GAP.zub = z;
            return z;
        }

        public int opt10(int[,] cost)
        {
            /*Si considera a turno ogni client e si prova a riassegnarlo ad ogni altro magazzino
            che ha capacità residua sufficiente.*/

            int z = 0, isol = 0;
            int[,] req = GAP.req;
            bool isImproved = true;

            int[] capleft = new int[m];
            for (int i = 0; i < m; i++) capleft[i] = GAP.cap[i]; //assegno capacità
            

            for(int j = 0; j< n; j++)
            {
                //capleft[GAP.sol[j]] -= GAP.req[GAP.sol[j], j]; //riduco le capacità
                z += GAP.cost[sol[j], j]; //soluzione: costo del server per il client
            }

            while (isImproved)
            {
                isImproved = false;
                for (int j = 0; j < n; j++)
                {         
                    for (int i = 0; i < m; i++)
                    {
                        isol = sol[j];
                        if ( i != isol && cost[i,j] < cost[isol, j] && capleft[i] >= req[i,j]){ //costo attuale < costo e ci sono capacità rimanenti

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
                System.Diagnostics.Debug.WriteLine("Solution is different of: " + Math.Abs(z - zcheck) + " should not be that!");
                return -1;
            }
            return z;
        }


       public int simulatedAnnealing()
       {
            int i, j, p = 0;
            int z = 0;
            int k = 10; //costante di Boltzmann

            int iter = 0;

            double maxTemp = 1000;
            int maxIter = 10;
            double alpha = 0.95; //geometric cooling
            double prob = 0.001;

            double temp = maxTemp;

            //DA QUI
            int initialCost = constructiveSolution();
            int firstCost = initialCost;

            //int[,] cost = GAP.cost;

            int[] capleft = new int[m];
            for (i = 0; i < m; i++) capleft[i] = GAP.cap[i]; //assegno capacità
            
            Random rand = new Random(100);

            while (iter < maxIter)
            {               
                iter++;
                z = firstCost;
                // generare sol casuale
                j = rand.Next(0, n - 1);    //clienti
                int isol = sol[j];      
                i = rand.Next(0, m - 1);    //magazzini

                int[] tmpSol = sol;
                tmpSol[j] = i;
                capleft[i] -= GAP.req[i, j];
                capleft[isol] += GAP.req[isol, j];

                int lastCost = firstCost;
                lastCost -= (GAP.cost[isol, j] - GAP.cost[i, j]);

                if (lastCost < firstCost)
                {
                    sol = tmpSol;
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
                        sol = tmpSol;
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

            int zcheck = checkSol(sol);
            if (z != zcheck)
            {
                z = int.MaxValue;
            }

            return z;
        }

    }
}
 