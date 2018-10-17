using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Blackjack
{
    public enum Suit
    {
        Coeur,
        Pique,
        Trefle,
        Carreau
    }

    public enum Face
    {
        Ace,
        Deux,
        Trois,
        Quatre,
        Cinq,
        Six,
        Sept,
        Huit,
        Neuf,
        Dix,
        Jack,
        Queen,
        King,
    }
    // Description des des valeurs(Obtenir et ensemble)
    public class Carte
    {
        public Suit Suit { get; set; }
        public Face Face { get; set; }
        public int Valeur { get; set; }
    }
    //Entrer de la plateforme de jeu
    public class plateforme
    {
        private List<Carte> cartes;

        public plateforme()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            cartes = new List<Carte>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    cartes.Add(new Carte() { Suit = (Suit)i, Face = (Face)j });

                    if (j <= 8)
                        cartes[cartes.Count - 1].Valeur = j + 1;
                    else
                        cartes[cartes.Count - 1].Valeur = 10;
                }
            }
        }

        public void melange()
        {
            Random rng = new Random();
            int n = cartes.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Carte carte = cartes[k];
                cartes[k] = cartes[n];
                cartes[n] = carte;
            }
        }

        public Carte PrendreCarte()
        {
            if (cartes.Count <= 0)
            {
                this.Initialize();
                this.melange();
            }

            Carte carteRetour = cartes[cartes.Count - 1];
            cartes.RemoveAt(cartes.Count - 1);
            return carteRetour;
        }

        public int MontantCartesRestantes()
        {
            return cartes.Count;
        }

        public void PrintDeck()
        {
            int i = 1;
            foreach (Carte carte in cartes)
            {
                Console.WriteLine("Carte {0}: {1} of {2}. Valeur: {3}:", i, carte.Face, carte.Suit, carte.Valeur);
                i++;
            }
        }



    }

    class Program
    {

        static int jetons;
        static plateforme plateforme;
        static List<Carte> player;
        static List<Carte> dealerIA;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Title = "♠♥♣♦ Blackjack Game by RN2629";
            Console.WriteLine("\nSoyez les Bienvenus dans le Jeu Stylé BlackJack!\n");
            // Initialisation des variables et debut du programme
            jetons = 100;
            plateforme = new plateforme();
            plateforme.melange();

            while (jetons > 0)
            {
                DealerIA();
                Console.WriteLine("\nAppuyez sur n'importe une touche pour continuer...\n");
                ConsoleKeyInfo playerInput = Console.ReadKey(true);
            }

            Console.WriteLine("Vous avez perdu! A la prochaine...");
            Console.ReadLine();
        }

        static void DealerIA()
        {
            if (plateforme.MontantCartesRestantes() < 20)
            {
                plateforme.Initialize();
                plateforme.melange();
            }

            Console.WriteLine("Cartes Restantes: {0}", plateforme.MontantCartesRestantes());
            Console.WriteLine("Jetons actuelles: {0}", jetons);
            Console.WriteLine("Combien voulez vous miser? (1 - {0})", jetons);
            string input = Console.ReadLine().Trim().Replace(" ", "");
            int montantParier;
            while (!Int32.TryParse(input, out montantParier) || montantParier < 1 || montantParier > jetons)
            {
                Console.WriteLine("Montant non pris en charge. Combien voulez vous miser?(1 - {0})", jetons);
                input = Console.ReadLine().Trim().Replace(" ", "");
            }
            Console.WriteLine();

            player = new List<Carte>();
            player.Add(plateforme.PrendreCarte());
            player.Add(plateforme.PrendreCarte());

            foreach (Carte carte in player)
            {
                if (carte.Face == Face.Ace)
                {
                    carte.Valeur += 10;
                    break;
                }
            }

            Console.WriteLine("[Player]");
            Console.WriteLine("Carte 1: {0} de {1}", player[0].Face, player[0].Suit);
            Console.WriteLine("Carte 2: {0} de {1}", player[1].Face, player[1].Suit);
            Console.WriteLine("Total: {0}\n", player[0].Valeur + player[1].Valeur);

            dealerIA = new List<Carte>();
            dealerIA.Add(plateforme.PrendreCarte());
            dealerIA.Add(plateforme.PrendreCarte());

            foreach (Carte carte in dealerIA)
            {
                if (carte.Face == Face.Ace)
                {
                    carte.Valeur += 10;
                    break;
                }
            }

            Console.WriteLine("[Dealer]");
            Console.WriteLine("Carte 1: {0} de {1}", dealerIA[0].Face, dealerIA[1].Suit);
            Console.WriteLine("Carte 2: [Carte Cachée]");
            Console.WriteLine("Total: {0}\n", dealerIA[0].Valeur);

            bool assurance = false; ;

            if (dealerIA[0].Face == Face.Ace)
            {
                Console.WriteLine("Assurance? (o / n)");
                string playerInput = Console.ReadLine();

                while (playerInput != "o" && playerInput != "n")
                {
                    Console.WriteLine("Voulez vous une Assurance? (o / n)");
                    playerInput = Console.ReadLine();
                }

                if (playerInput == "o")
                {
                    assurance = true;
                    //chips -= betAmount / 2;
                    Console.WriteLine("\n[Assurance Acceptée!]\n");
                }
                else
                {
                    assurance = false;
                    Console.WriteLine("\n[Assurance Rejeté]\n");
                }
            }

            if (dealerIA[0].Face == Face.Ace || dealerIA[0].Valeur == 10)
            {
                Console.WriteLine("IA vérifie s'il a un Blackjack...\n");
                Thread.Sleep(2000);
                if (dealerIA[0].Valeur + dealerIA[1].Valeur == 21)
                {
                    Console.WriteLine("[Dealer]");
                    Console.WriteLine("Carte 1: {0} de {1}", dealerIA[0].Face, dealerIA[1].Suit);
                    Console.WriteLine("Carte 2: {0} de {1}", dealerIA[1].Face, dealerIA[1].Suit);
                    Console.WriteLine("Total: {0}\n", dealerIA[0].Valeur + dealerIA[1].Valeur);

                    Thread.Sleep(2000);

                    int montantPerdu = 0;

                    if (player[0].Valeur + player[1].Valeur == 21 && assurance)
                    {
                        montantPerdu = montantParier / 2;
                        jetons -= montantParier / 2;
                    }
                    else if (player[0].Valeur + player[1].Valeur != 21 && !assurance)
                    {
                        montantPerdu = montantParier + montantParier / 2;
                        jetons -= montantParier + montantParier / 2;
                    }

                    Console.WriteLine("Vous avez perdu, votre  {0} montant", montantPerdu);

                    Thread.Sleep(1000);

                    return;
                }
                else
                {
                    Console.WriteLine("IA n'as pas de Blackjack, continuer...\n");
                }
            }

            if (player[0].Valeur + player[1].Valeur == 21)
            {
                Console.WriteLine("Blackjack, Vous avez gagné! ({0} jetons)\n", montantParier + montantParier / 2);
                jetons += montantParier + montantParier / 2;
                return;
            }

            do
            {
                Console.WriteLine("Choisissez une option valide SVP: [(C)Continuer (Q)Quitter]");
                ConsoleKeyInfo playerOption = Console.ReadKey(true);
                while (playerOption.Key != ConsoleKey.Q && playerOption.Key != ConsoleKey.C)
                {
                    Console.WriteLine("Option Invalide: [(C)Continuer (Q)Quitter]");
                    playerOption = Console.ReadKey(true);
                }
                Console.WriteLine();

                switch (playerOption.Key)
                {
                    case ConsoleKey.Q:
                        player.Add(plateforme.PrendreCarte());
                        Console.WriteLine("Quitter {0} de {1}", player[player.Count - 1].Face, player[player.Count - 1].Suit);
                        int valeursCartestotals = 0;
                        foreach (Carte carte in player)
                        {
                            valeursCartestotals += carte.Valeur;
                        }
                        Console.WriteLine("Valeur total des cartes: {0}\n", valeursCartestotals);
                        if (valeursCartestotals > 21)
                        {
                            Console.Write("Busted!\n");
                            jetons -= montantParier;
                            Thread.Sleep(2000);
                            return;
                        }
                        else if (valeursCartestotals == 21)
                        {
                            Console.WriteLine("Bien Joué!Je suppose que vous voulez continuer...\n");
                            Thread.Sleep(2000);
                            continue;
                        }
                        else
                        {
                            continue;
                        }


                    case ConsoleKey.C:

                        Console.WriteLine("[Dealer]");
                        Console.WriteLine("Card 1: {0} de {1}", dealerIA[0].Face, dealerIA[1].Suit);
                        Console.WriteLine("Card 2: {0} de {1}", dealerIA[1].Face, dealerIA[1].Suit);

                        int dealerValeursCartes = 0;
                        foreach (Carte carte in dealerIA)
                        {
                            dealerValeursCartes += carte.Valeur;
                        }

                        while (dealerValeursCartes < 17)
                        {
                            Thread.Sleep(2000);
                            dealerIA.Add(plateforme.PrendreCarte());
                            dealerValeursCartes = 0;
                            foreach (Carte carte in dealerIA)
                            {
                                dealerValeursCartes += carte.Valeur;
                            }
                            Console.WriteLine("Carte {0}: {1} de {2}", dealerIA.Count, dealerIA[dealerIA.Count - 1].Face, dealerIA[dealerIA.Count - 1].Suit);
                        }
                        dealerValeursCartes = 0;
                        foreach (Carte carte in dealerIA)
                        {
                            dealerValeursCartes += carte.Valeur;
                        }
                        Console.WriteLine("Total: {0}\n", dealerValeursCartes);

                        if (dealerValeursCartes > 21)
                        {
                            Console.WriteLine("IA perds! Vous gagnez! ({0} jetons)", montantParier);
                            jetons += montantParier;
                            return;
                        }
                        else
                        {
                            int playerValeurCarte = 0;
                            foreach (Carte card in player)
                            {
                                playerValeurCarte += card.Valeur;
                            }

                            if (dealerValeursCartes > playerValeurCarte)
                            {
                                Console.WriteLine("IA a {0} et vous avez {1}, IA gagne!", dealerValeursCartes, playerValeurCarte);
                                jetons -= montantParier;
                                return;
                            }
                            else
                            {
                                Console.WriteLine("VOus avez {0} et IA a {1}, Vous gagnez!", playerValeurCarte, dealerValeursCartes);
                                jetons += montantParier;
                                return;
                            }
                        }


                    default:
                        break;
                }

                Console.ReadLine();
            }
            while (true);
        }
    }
}
