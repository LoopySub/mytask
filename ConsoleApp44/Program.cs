namespace ConsoleApp44
{
 
    public class Program
    {
        static void Main()
        {
            Player player = new Player("Chad", "전사", 10, 5, 100, 1500);
            Shop shop = new Shop();
            bool open = false;
            bool isMain = true;

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

            while (true)
            {
                if (!open)
                {
                    Console.WriteLine("\n1. 상태 보기\n2. 인벤토리\n3. 상점\n0. 종료");
                    Console.Write("원하시는 행동을 입력해주세요: ");
                }
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        ShowStatus(player);
                        open = !open;
                        isMain = !isMain;
                        break;
                    case "2":
                        Console.Clear();
                        ShowInventory(player);
                        open = !open;
                        isMain = !isMain;
                        break;
                    case "3":
                        Console.Clear();
                        ShowShop(shop, player);
                        open = !open;
                        isMain = !isMain;
                        break;
                    case "0":
                        if (isMain == true)
                        {
                            Console.WriteLine("게임을 종료합니다. 안녕히 가세요!");
                            return;
                            break;
                           
                        }
                        else
                        {
                            Main();
                        }
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        break;
                }
            }
        }
        static void ShowStatus(Player player)
        {
            Console.WriteLine("\n상태 보기\n");
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ( {player.Job} )");
            Console.WriteLine($"공격력: {player.Attack} | 방어력: {player.Defense} | 체력: {player.Health}");
            Console.WriteLine($"Gold: {player.Gold} G");
            Console.WriteLine("\n0. 나가기");
            string input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    Console.Clear();
                    Main();
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    break;
            }
        }

        static void ShowInventory(Player player)
        {
            Console.WriteLine("\n인벤토리\n");
            Console.WriteLine("[인벤토리 아이템 목록]");
            foreach (Item item in player.Inventory)
            {
                Console.WriteLine($"{item.Name} | 가격: {item.Price} G | {item.Description}");
            }

            Console.WriteLine("\n0. 나가기");
            string input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    Console.Clear();
                    Main();
                    break;
               
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    break;
            }
        }

        static void ShowShop(Shop shop, Player player)
        {
            Console.WriteLine("\n상점\n");
            Console.WriteLine($"[보유 골드]\n{player.Gold} G\n");

            // 상점 아이템 목록 표시
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < shop.Items.Count; i++)
            {
                Item item = shop.Items[i];
                Console.WriteLine($"{i + 1}. {item.Name} | 가격: {item.Price} G | {item.Description}");
            }

            Console.Write("\n1. 아이템 구매\n0. 나가기\n");

            Console.Write("원하시는 행동을 입력해주세요: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    BuyItem(shop, player);
                    break;
                case "0":
                    Console.Clear();
                    Main();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    break;
            }
        }

        static void BuyItem(Shop shop, Player player)
        {
            Console.WriteLine("\n상점 - 아이템 구매");
            Console.WriteLine($"[보유 골드]\n{player.Gold} G\n");

            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < shop.Items.Count; i++)
            {
                Item item = shop.Items[i];
                Console.WriteLine($"{i + 1}. {item.Name} | 가격: {item.Price} G | {item.Description}");
            }

            Console.Write("\n구매할 아이템 번호를 입력해주세요 (0. 나가기): ");
            string input = Console.ReadLine();

            int selectedItemIndex;
            if (int.TryParse(input, out selectedItemIndex) && selectedItemIndex >= 1 && selectedItemIndex <= shop.Items.Count)
            {
                Item selectedItem = shop.Items[selectedItemIndex - 1];

                if (selectedItem.Price <= player.Gold)
                {
                    Console.WriteLine($"구매를 완료했습니다. {selectedItem.Name}을(를) 획득하였습니다.");
                    player.Gold -= selectedItem.Price;
                    player.Inventory.Add(selectedItem);
                    ShowInventory(player);
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }
            else if (selectedItemIndex == 0)
            {
                Console.Clear();
                ShowShop(shop, player);
              
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
            }
        }
    }

    class Player
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }
        public List<Item> Inventory { get; set; } = new List<Item>();

        public Player(string name, string job, int attack, int defense, int health, int gold) // 왜 매개변수를 받는 생성자를 썼는지? 
        {
            Level = 1;
            Name = name;
            Job = job;
            Attack = attack;
            Defense = defense;
            Health = health;
            Gold = gold;
        }
    }

    class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        public Item(string name, int price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }
    }

    class Shop
    {
        public List<Item> Items { get; set; }
        public Shop()
        {
            Items = new List<Item>
        {
            new Item("수련자 갑옷", 1000, "수련에 도움을 주는 갑옷입니다."),
            new Item("무쇠갑옷", 1500, "무쇠로 만들어져 튼튼한 갑옷입니다."),
            new Item("스파르타의 갑옷", 3500, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."),
            new Item("낡은 검", 600, "쉽게 볼 수 있는 낡은 검 입니다."),
            new Item("청동 도끼", 1500, "어디선가 사용됐던거 같은 도끼입니다."),
            new Item("스파르타의 창", 2000, "스파르타의 전사들이 사용했다는 전설의 창입니다.")
        };
        }
    }

}
