using System;
using System.Text;
using System.Security.Cryptography;
namespace TexasHoldem
{

    class Database
    {
        public Database() {; }
        public void AddAccount(string username, string password, int credits)
        {
            
        }
        public bool CheckCredentials(string username, string password)
        {
            return false;
        }
        public int GetCredits(string username)
        {
            return 0;
        }
        public void UpdateCredits(string username, int credits)
        {
            ;
        }
    }
    enum Suit
    {
        diamonds = 1,clubs,hearts,spades
    }
    enum CardType
    {
        _2 = 2,_3,_4,_5,_6,_7,_8,_9,_10,_J,_Q,_K,_A,
    }
    sealed class Card
    {
        public Card(CardType type, Suit suit)
        {
            this.suit = suit; this.type = type;
        }
        public Suit suit;
        public CardType type;
    }
    sealed class TexasHoldem
    {
        static string ReadWithWrite(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public TexasHoldem() {; }
        Card[] pack = new Card[52];
        private int currTop = 0;
        private int MinimumBet = 10;
        private int _playerCredits;
        private bool _anonymous;
        private string _username;
        private bool _dealerBetStart;
        private int lastDealerBet = 0;
        Card[] knownCards = new Card[5];
        Card[] dealer = new Card[2];
        private int _;
        private int pot = 0;
        private const int InitialCredits = 1000;
        private static Database db = new Database();
        private double EvaluateChances()
        {
            return 100.0;
        }
        private void InitPack()
        {
            for (CardType i = CardType._2; i <= CardType._A; i++)
            {
                for (Suit j = Suit.diamonds; j <= Suit.spades; j++)
                {
                    int index = ((int)i - 2) * 4 + (int)j - 1;
                    pack[index] = new Card(i,j);
                }          
            }
        }
        public Card[] GetCards(int amount)
        {
            Card[] cards = new Card[amount];
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = pack[currTop++];
            }
            return cards;
        }
        public void ShufflePack()
        {
            Random rnd = new Random();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 52; i++)
                {
                    Card tempCard;
                    int tempIdx = rnd.Next(i, 52);
                    tempCard = pack[tempIdx];
                    pack[tempIdx] = pack[i];
                    pack[i] = tempCard;
                }
                for (int i = 51; i >= 0; i--)
                {
                    Card tempCard;
                    int tempIdx = rnd.Next(0, i);
                    tempCard = pack[tempIdx];
                    pack[tempIdx] = pack[i];
                    pack[i] = tempCard;
                }
            }
        }
        public void broadcastBet(string who, int amount)
        {
            if (who == "Dealer")
                lastDealerBet = amount;
            Console.WriteLine($"{who} bet {amount}");
            pot += amount;
        }
        public void StartGame(bool anonymous, int PlayerCredits, string username,
            bool dealerBetStart = true)
        {
            _playerCredits = PlayerCredits; _anonymous = anonymous; _username = username;
            _dealerBetStart = dealerBetStart;
            if (PlayerCredits < MinimumBet)
            {
                Console.WriteLine("Insufficient funds to play.");
                return;
            }
            Console.Clear();
            InitPack();
            ShufflePack();
            Card[] playerCards = GetCards(2);
            dealer = GetCards(2);
            Console.Write("Your Cards: "); printCards(playerCards);
            Console.WriteLine();
            if (_dealerBetStart)
            {
                broadcastBet("Dealer", MinimumBet);
                broadcastBet(username, MinimumBet / 2);
                _playerCredits -= 5;
                lastDealerBet -= 5;
            }
            else
            {
                broadcastBet(username, MinimumBet);
                _playerCredits -= 10;
                broadcastBet("Dealer", MinimumBet / 2);
            }
            if (_dealerBetStart)
                AskWhatNext();
            Card[] flop = GetCards(3);
            for (int i = 0; i < 3; i++) knownCards[i] = flop[i];
            Console.Write("FLOP: "); printCards(knownCards);
            if (_dealerBetStart)
                AskWhatNext();
            else
            {
                if (EvaluateChances() > 50.0)
                    broadcastBet("Dealer", (int)Math.Round(calcAllowance()));
                AskWhatNext();
            }
            Card[] turn = GetCards(1); knownCards[3] = turn[0];
            Console.Write("TURN: "); printCards(knownCards);
            if (_dealerBetStart)
                AskWhatNext();
            else
            {
                if (EvaluateChances() > 50.0)
                    broadcastBet("Dealer", (int)Math.Round(calcAllowance()));
                AskWhatNext();
            }
            Card[] river = GetCards(1); knownCards[4] = river[0];
            Console.Write("RIVER: "); printCards(knownCards);
            if (_dealerBetStart)
                AskWhatNext();
            else
            {
                if (EvaluateChances() > 50.0)
                    broadcastBet("Dealer", (int)Math.Round(calcAllowance()));
                AskWhatNext();
            }


            Console.ReadKey();
        }
        public double calcAllowance()
        {
            double eval = EvaluateChances();
            return _playerCredits * eval / 100.0;
        }
        public void AskWhatNext()
        {
            Exception e; string answer;
        Ask:
            if (lastDealerBet == 0)
                answer = ReadWithWrite("Fold, Raise? (f/r) ");
            else 
                answer = ReadWithWrite("Fold, Call, Raise? (f/c/r) ");
            if (answer[0] == 'f')
                GameEnd();
            else if (answer[0] == 'c' && lastDealerBet != 0)
            {
                (e, _playerCredits) = SafeSubtract(_playerCredits, lastDealerBet);
                if (e != null)
                    goto Ask;
                lastDealerBet = 0;
            }
            else if (answer[0] == 'r')
            {
                string toRaiseStr = ReadWithWrite("How much to bet? ");
                int toRaise = Convert.ToInt32(toRaiseStr);
                if (toRaise < lastDealerBet)
                    goto Ask;
                if (toRaise > calcAllowance() * 2 && _playerCredits >= toRaise)
                {
                    GameEnd(true);
                }
                (e, _playerCredits) = SafeSubtract(_playerCredits, toRaise);
                if (e != null)
                    goto Ask;

                lastDealerBet = 0;
            }
            else
                goto Ask;
        }
        public (Exception,int) SafeSubtract(int a, int b)
        {
            if (b > a)
            {
                Console.WriteLine("insufficient funds");
                return (new Exception("funds"), a);
            }
            else
                return (null, a - b);
        }
        public void GameEnd(bool playerWin = false)
        {
            if (playerWin)
                _playerCredits += pot;
            if (!_anonymous)
                db.UpdateCredits(_username, _playerCredits);
            ReplayAsk:
            string response = ReadWithWrite("Replay? (y/n) ");
            if (response[0] == 'y')
            {
                pot = 0; ShufflePack(); currTop = 0;
                StartGame(_anonymous, (_anonymous) ? InitialCredits : _playerCredits, _username,
                    !_dealerBetStart);
            }
            else if (response[0] == 'n')
                Environment.Exit(0);
            else
                goto ReplayAsk;
            
        }
        public string CardToString(Card card)
        {
            StringBuilder bob = new StringBuilder();
            bob.Append(card.suit.ToString()[0]).Append(card.type.ToString()[1]);
            return bob.ToString();
        }
        public void printCards(Card[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            { 
                ConsoleColor color = (cards[i].suit == Suit.diamonds || cards[i].suit == Suit.hearts) ?
                ConsoleColor.Red : ConsoleColor.DarkMagenta;
                Console.ForegroundColor = color;
                Console.Write($"{CardToString(cards[i])} ");
                if ((i + 1) % 13 == 0)
                    Console.WriteLine();
            }
            ResetColor();
        }
        public void ResetColor() { Console.ForegroundColor = ConsoleColor.Yellow; }
    }
    class Program
    {
        private static SHA256 sha2;
        private const int InitialCredits = 1000;
        private static Database db = new Database();
        static string ReadWithWrite(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
        static string CalcHash(string text)
        {
            byte[] result = sha2.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder Bob = new StringBuilder();
            foreach (byte b in result)
                Bob.Append(b.ToString("x2")); // hexadecimal format
       
            return Bob.ToString();

        }
        static string InitAccount()
        {
            Console.WriteLine("[Account Creation]");
            string username = ReadWithWrite("[Username]: ");
            string password = ReadWithWrite("[Password]: ");
            password = CalcHash(password);
            int credits = InitialCredits;
            db.AddAccount(username, password, credits);
            return username;
        }
        static (string, int) LoginProcess()
        {
            tryLogin:
            string username = ReadWithWrite("[Username]: ");
            string password = ReadWithWrite("[Password]: ");
            password = CalcHash(password);
            if (!db.CheckCredentials(username, password))
            {
                Console.WriteLine("[Error]: false credentials");
                goto tryLogin;
            }
            int credits = db.GetCredits(username);
            return (username, credits);

        }
        static void Main(string[] args)
        {
            // init database
            bool anonymous = false; int PlayerCredits = InitialCredits; string username = "Anonymous";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to PokerPoker!");
            
           AskLogin:
               string response = ReadWithWrite("Login? (y/n) ");
               if (response[0] == 'y')
               {
                   (username,PlayerCredits) = LoginProcess();
               }
               else if (response[0] == 'n')
               {
               AskAnonymous:
                   string answer = ReadWithWrite("Play anonymous? (y/n) ");
                   if (answer[0] == 'y')
                       anonymous = true;
                   else if (answer[0] == 'n')
                       username = InitAccount();
                   else
                       goto AskAnonymous;
               }
               else
                   goto AskLogin; 
            TexasHoldem game = new TexasHoldem();
            game.StartGame(anonymous,PlayerCredits,username);
        }
    }
}