using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace The_PROJECT_2__Bank_
{
    internal class BankATM
    {
        static string accnumb, nip;
        static Int16 nbclient, attemps, choice;
        static bool endOfProgram = true;
        struct clients
        {
            public string numero;
            public string name;
            public string nip;
            public decimal balance;
        }
        static clients[] tabclient = new clients[200];
        static void Main(string[] args)
        {
            while (endOfProgram)
            {
                Console.Clear();
                readfromfile();
                displaytitleandsub("banque royale", "Guichet Automatique Bancaire");
                TakeUserInputs();
            }
        }


        private static void TakeUserInputs()
        {
            Console.Write("Entrez votre numéro de compte : ");
            accnumb = Console.ReadLine().ToUpper();
            bool accExist = checkaccountexist();

            for (Int16 i = 0; i < nbclient; i++)

            {
                if (accExist && accnumb == tabclient[i].numero)
                {
                    bool passIsCorrect = checkpasswordIsCorrect();
                    if (passIsCorrect)
                    {
                        Int16 userchoice = takeChoice();
                        switch (userchoice)
                        {
                            case 1:
                                deposit();
                                displayinfo();
                                break;
                            case 2:
                                withdraw();
                                displayinfo();
                                break;
                            case 3:
                                displayinfo();
                                break;
                        }
                    }
                }
            }
        }
        private static void withdraw()
        {
            decimal withdraw;
            bool checkcorrectamount = true;

            for (Int16 i = 0; i < nbclient; i++)
            {
                if (accnumb == tabclient[i].numero)
                {
                    Console.WriteLine("\nVous avez " + tabclient[i].balance + "$ sur votre compte bancaire");
                    while (checkcorrectamount)
                    {
                        Console.Write("\nEntrez le montant à retirer $: ");
                        withdraw = Convert.ToDecimal(Console.ReadLine());
                        if (withdraw < 20)
                        {
                            Console.Write("ERREUR!!! Le minimum de retrait est de 20$. Appuyez sur une touche pour réessayer ");
                            Console.ReadKey();
                        }
                        else if (withdraw > 500)
                        {
                            Console.Write("ERREUR!!! Le maximum de retrait est de 500$. Appuyez sur une touche pour réessayer ");
                            Console.ReadKey();
                        }
                        else if (withdraw > tabclient[i].balance)
                        {
                            Console.Write("ERREUR!!! Le montant doit être inférieur au compte bancaire. Appuyez sur une touche pour réessayer ");
                            Console.ReadKey();
                        }
                        else if (withdraw % 20 != 0)
                        {
                            Console.Write("ERREUR!!! Le montant doit être un multiple de 20. Appuyez sur une touche pour réessayer ");
                            Console.ReadKey();
                        }
                        if (withdraw >= 20 && withdraw <= 500 && withdraw % 20 == 0 && withdraw <= tabclient[i].balance)
                        {
                            tabclient[i].balance = tabclient[i].balance - withdraw;
                            Console.WriteLine("\n    ---  La transaction a réussi ---");
                            writetofile();
                            break;
                        }
                    }
                }
            }
        }
        private static void deposit()
        {
            decimal deposit;
            for (Int16 i = 0; i < nbclient; i++)
            {
                if (accnumb == tabclient[i].numero)
                {
                    Console.WriteLine("Vous avez " + tabclient[i].balance + "$ sur votre compte bancaire");
                    Console.Write("Entrez le montant que vous souhaitez déposer : ");
                    deposit = Convert.ToDecimal(Console.ReadLine());
                    while (deposit < 2 || deposit > 20000)
                    {
                        Console.Write("Le montant n'est pas correct, veuillez saisir un chiffre compris entre 2 et 20 000 : ");
                        deposit = Convert.ToDecimal(Console.ReadLine());
                    }
                    tabclient[i].balance = tabclient[i].balance + deposit;
                    writetofile();
                    Console.WriteLine("\n ---  La transaction a reussi ---");
                    Console.WriteLine("vous avez déposé " + deposit + "$ sur votre compte\n");
                }
            }
        }
        private static void displayinfo()
        {
            for (Int16 i = 0; i < nbclient; i++)
            {
                if (accnumb == tabclient[i].numero)
                {
                    Console.WriteLine("\nLes infos du compte");
                    Console.WriteLine("\tNumero : " + tabclient[i].numero);
                    Console.WriteLine("\tClient : " + tabclient[i].name);
                    Console.WriteLine("\tNip : " + tabclient[i].nip);
                    Console.WriteLine("\tSolde $ : " + tabclient[i].balance);
                    Console.WriteLine("\n");
                    Console.WriteLine("Merci d'avoir utiliser nos service");
                    Console.Write("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
                }
            }
        }
        private static Int16 takeChoice()
        {
            Console.WriteLine("\nChoisir votre Transaction");
            Console.WriteLine("\t1 - Pour Déposer");
            Console.WriteLine("\t2 - Pour Retirer");
            Console.WriteLine("\t3 - Pour Consulter");
            Console.Write("Entrez votre choix <1 - 3> : ");
            choice = Convert.ToInt16(Console.ReadLine());
            while (choice < 1 || choice > 3)
            {
                Console.Write("Mauvais input, entrez votre choix <1 - 3> : ");
                choice = Convert.ToInt16(Console.ReadLine());
            }
            return choice;
        }
        private static bool checkaccountexist()
        {
            for (Int16 i = 0; i < nbclient; i++)
            {
                if (accnumb == tabclient[i].numero)
                {
                    Console.WriteLine("\n\tBienvenue " + tabclient[i].name);
                    return true;
                }
            }
            Console.Write("Votre compte n'existe pas, veuillez réessayer ");
            Console.ReadKey();
            return false;
        }
        private static bool checkpasswordIsCorrect()
        {
            bool correct = false;
            attemps = 3;
            while (attemps > 0)
            {
                Console.Write("Entrez votre nip : ");
                nip = Console.ReadLine();
                for (Int16 i = 0; i < nbclient; i++)
                {
                    if (accnumb == tabclient[i].numero && nip == tabclient[i].nip)
                    {
                        return true;
                    }
                }
                if (correct == false)
                {
                    attemps--;
                    Console.WriteLine("Le nip n'est pas correct, veuillez réessayer");
                    Console.WriteLine("Il te reste " + attemps + " essais");

                }
            }
            if (attemps == 0)
            {
                Console.Write("Vous avez atteint la limite, veuillez réessayer plus tard ");
                Console.ReadKey();
            }
            return false;
        }
        private static void writetofile()
        {
            StreamWriter myfile2 = new StreamWriter("clients.txt");
            for (Int16 i = 0; i < nbclient; i++)
            {
                myfile2.WriteLine(tabclient[i].numero);
                myfile2.WriteLine(tabclient[i].name);
                myfile2.WriteLine(tabclient[i].nip);
                myfile2.WriteLine(tabclient[i].balance);
            }
            myfile2.Close();
        }
        private static void readfromfile()
        {

            StreamReader myfile = new StreamReader("clients.txt");
            Int16 i = 0;
            while (myfile.EndOfStream == false)
            {
                tabclient[i].numero = myfile.ReadLine();
                tabclient[i].name = myfile.ReadLine();
                tabclient[i].nip = myfile.ReadLine();
                tabclient[i].balance = Convert.ToDecimal(myfile.ReadLine());
                i++;
            }
            nbclient = i;
            myfile.Close();
        }
        private static void displaytitleandsub(string tit, string sub)
        {

            Console.WriteLine("\t\t" + tit.ToUpper());
            Console.Write("\t\t");
            for (Int16 i = 0; i < tit.Length; i++)
            {
                Console.Write("_");
            }
            Console.Write("\n");
            Console.WriteLine("\t" + sub);
            Console.Write("\t");
            for (Int16 i = 0; i < sub.Length; i++)
            {
                Console.Write("_");

            }
            Console.WriteLine("\n");
        }

    }
}
