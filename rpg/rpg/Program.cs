// See https://aka.ms/new-console-template for more information

using System;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;


namespace rpg
{
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int HP { get; }
        public int Gold { get; set; }
        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            HP = hp;
            Gold = gold;

        }
    }
    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        public int Type { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }

        public int Gold { get; set; }

        public bool IsEquipped { get; set; }  // 장착이 실제로 되었는지 확인

        public bool IsPurchased { get; set; }
        public static int itemCnt = 0;        //Item이라는 Class에 귀속, 객체 생성 없이도 메서드나 변수를 사용할 수 있습니다
        public Item(string name, string description, int type, int atk, int def, int hp,int gold, bool isEuquipped = false)
        {
            Name = name;
            Description = description;
            Type = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            IsEquipped = isEuquipped;

        }
        public void PrintItem(bool withNumber = false, int idx = 0 ,bool showPrice = true)
        {
            Console.Write("- ");

            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0}", idx);
                Console.ResetColor();
            }
            if (IsEquipped)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");

            }
            Console.Write(Name);
            Console.Write(" | ");

            if (Atk != 0) Console.Write($"Atk{(Atk >= 0 ? " + " : "")}{Atk}");  //삼항연산자 ?앞의 조건이 참이라면 "+"를 쓰고 아니라면 ""를 쓴다.
            if (Def != 0) Console.Write($"Def{(Def >= 0 ? " + " : "")}{Def}");
            if (Hp != 0) Console.Write($"Hp{(Hp >= 0 ? " + " : "")}{Hp}");
       
            Console.Write(" | ");
            Console.Write(Description);

            if (showPrice) // 8. 상점관련, 가격 및 구매 완료 표시 bool 코드
            {
                // IsPurchased가 true이면 "구매완료", 아니면 가격을 출력
                string priceOrStatus = IsPurchased ? "구매완료" : Gold.ToString() + " G";
                Console.Write(" | " + priceOrStatus);
            }
            Console.WriteLine();

        }


        internal class Program
        {
            static Character _player;
            static Item[] _item;
            static void Main()
            {

                GameDataSetting();
                PrintStartLogo();
                StartMenu();
            }

            private static void StartMenu()
            {
                Console.Clear();
                Console.WriteLine("===========================================================================");
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine("===========================================================================");
                Console.WriteLine("");
                Console.WriteLine("1.상태보기");
                Console.WriteLine("2.인벤토리");
                Console.WriteLine("3.상점");
                Console.WriteLine("");
                switch (CheckValidInput(1, 3))
                {
                    case 1:
                        StatusMenu();
                        break;
                    case 2:
                        InventoryMenu();
                        break;
                    case 3:
                        StoreMenu();
                        break;
                }
            }

            private static void StoreMenu()
            {
                Console.Clear();
                ShowHl("■ 상점 ■");
                Console.WriteLine("아이템을 살 수 있는 상점입니다.");
                Console.WriteLine("");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine("");
                TextHl("", _player.Gold.ToString(), " G"); 
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine("");
                for (int i = 0; i < Item.itemCnt; i++)
                {
                    _item[i].PrintItem(false, i + 1, true);

                }
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("");
                switch (CheckValidInput(0, 2))
                {
                    case 0:
                        StartMenu();
                        break;
                    case 1:
                        BuyItemMenu();
                        break;
                   
                }

            }

            private static void BuyItemMenu()
            {
                Console.Clear();
                ShowHl("■ 상 점 - 구매하기 ■");
                Console.WriteLine("필요한 아이템을 구매 할 수 있습니다.\n");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine("");
                for (int i = 0; i < Item.itemCnt; i++)
                {
                        _item[i].PrintItem(true, i + 1);
                 
                }
                Console.WriteLine("\n구매하고 싶은 아이템 번호를 입력 해주세요.");
                Console.WriteLine("0을 입력하면 상점으로 돌아갑니다.");
                Console.WriteLine("");

                int choice = CheckValidInput(0, _item.Length); // 0 입력 가능
                if (choice == 0) // 0 입력 시 상점 메뉴로 복귀
                {
                    StoreMenu();
                    return;
                }

                choice -= 1; // 사용자 입력에서 1을 빼서 실제 인덱스로
                Item selectedItem = _item[choice];
                if (selectedItem.IsPurchased)   // 이미 구매한 아이템인지 확인
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else if (_player.Gold >= selectedItem.Gold) // 플레이어 골드가 선택 아이템 골드보다 크면, 구매 가능한 경우
                {
                    selectedItem.IsPurchased = true; // 구매 표시를 true로

                    _player.Gold -= selectedItem.Gold; // 플레이어 골드 감소

                    Console.WriteLine($"\n{selectedItem.Name} 구매를 완료했습니다.");
                }
                else // 골드가 부족한 경우
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
               
                Console.WriteLine("아무 키나 누르면, 상점으로 돌아갑니다.");
                Console.ReadKey();
                StoreMenu();
            }

            private static void InventoryMenu()
            {
                Console.Clear();
                ShowHl("■ 인벤토리 ■");
                Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");

                for(int i = 0; i<Item.itemCnt; i++)
                {
                    if (_item[i].IsPurchased)
                    {
                        _item[i].PrintItem(true, i + 1, false);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("0: 나가기");
                Console.WriteLine("1: 장착관리");
                Console.WriteLine("");

                switch (CheckValidInput(0, 1))
                {
                    case 0:
                        StartMenu();
                        break;
                    case 1:
                        EquipMenu();
                        break;
                }
            }

            private static void EquipMenu()
            {
                Console.Clear();
                ShowHl("■ 인벤토리 - 장착관리 ■");
                Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < Item.itemCnt; i++)
                {
                    if (_item[i].IsPurchased) // 구매한 아이템 이라면 표시
                    {
                        _item[i].PrintItem(true, i + 1, false);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("0: 나가기");
                Console.WriteLine("장착하고싶은 무기의 숫자를 입력해주세요");

                int keyinput = CheckValidInput(0, Item.itemCnt);
                switch (keyinput)
                {
                    case 0:
                        InventoryMenu();
                        break;
                    default:
                        Toggle(keyinput - 1);
                        EquipMenu();
                        break;
                }
            }

            private static void Toggle(int idx)
            {
                _item[idx].IsEquipped = !_item[idx].IsEquipped;
            }

            private static void StatusMenu()
            {
                Console.Clear();
                ShowHl("■ 상태보기 ■");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                TextHl("Lv", _player.Level.ToString("00"));
                Console.WriteLine("");
                Console.WriteLine("{0}({1})", _player.Name, _player.Job);


                int bounusAtk = getSumAtk();
                TextHl("공격력: ", (_player.Atk + bounusAtk).ToString(), bounusAtk > 0 ? string.Format(" (+{0})", bounusAtk) : "");
                int bounusDef = getSumDef();
                TextHl("방어력: ", (_player.Def + bounusDef).ToString(), bounusDef > 0 ? string.Format(" (+{0})", bounusDef) : "");
                int bounusHp = getSumHp();
                TextHl("체력: ", (_player.HP + bounusHp).ToString(), bounusHp > 0 ? string.Format(" (+{0})", bounusHp) : "");
                TextHl("골드: ", _player.Gold.ToString());
                Console.WriteLine("");
                Console.WriteLine("0: 뒤로가기");
                Console.WriteLine("");
                switch (CheckValidInput(0,0))
                {
                    case 0:
                        StartMenu();
                        break;
                  

                }
            }

            private static  int getSumAtk()
            {
                int sum = 0;
                for (int i = 0; i < Item.itemCnt; i++)
                {
                    if (_item[i].IsEquipped) sum += _item[i].Atk;
                }
                  return sum;    
            }
            private static  int getSumDef()
            {
                int sum = 0;
                for (int i = 0; i < Item.itemCnt; i++)
                {
                    if (_item[i].IsEquipped) sum += _item[i].Def;
                }
                return sum;
            }
            private static int getSumHp()
            {
                int sum = 0;
                for (int i = 0; i < Item.itemCnt; i++)
                {
                    if (_item[i].IsEquipped) sum += _item[i].Hp;
                }
                return sum;
            }

            private static void ShowHl(string text)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            private static void TextHl(string s1, string s2, string s3 = "")
            {
                Console.Write(s1);
                Console.ForegroundColor= ConsoleColor.Red;
                Console.Write(s2);
                Console.ResetColor();
                Console.WriteLine(s3);
            }

            private static int CheckValidInput(int min, int max)
            {
                int keyinput;
                bool result;
                do
                {
                    Console.WriteLine("원하시는 행동을 입력해주세요");
                    result = int.TryParse(Console.ReadLine(),out keyinput);
                }
                while (result == false || CheckIfValid(keyinput, min, max) ==false);
                return keyinput;
            }

            private static bool CheckIfValid(int keyinput, int min, int max)
            {
                if(min <= keyinput && keyinput <= max) return true;
                return false;
            }

            private static void PrintStartLogo()
            {
                Console.WriteLine("    ________ ____           ");
                Console.WriteLine("\\______ \\   __ __   ____    / ___\\   ____ ____    ____   ");
                Console.WriteLine(" |    |  \\ |  |  \\ /    \\  / /_/  >_/ __ \\ /  _ \\  /    \\  ");
                Console.WriteLine(" |    `   \\|  |  /|   |  \\ \\___  / \\  ___/(  <_> )|   |  \\ ");
                Console.WriteLine("/_______  /|____/ |___|  //_____/   \\___  >\\____/ |___|  / ");
                Console.WriteLine("        \\/             \\/               \\/             \\/  ");
                Console.WriteLine("===========================================================================");
                Console.WriteLine("==========================press anykey to start============================");
                Console.WriteLine("===========================================================================");
                Console.ReadKey();
            }
               
                                                           
                                                                            
                                                                            
                                                                            

            private static void GameDataSetting()
            {
                _player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);
                _item = new Item[10];
                Additem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1,0,10,0,1000 ));
                Additem(new Item("낡은검", "쉽게 볼수있는 낡은검입니다.", 1, 10,0,0,1000 ));
                Additem(new Item("파괴의검", "모든것을 파괴하는 파괴의검.", 1, 100, 0, 0, 50000));
                Additem(new Item("청동도끼", "어디선가 사용됐던거 같은 도끼.", 1,20 , 0, 0, 1000));
            }
            static bool Additem(Item item)   //(Item item)을 추가하여 Item이 생성될떄 마다,string name, string description, int type, int atk, int def, int hp, bool isEuquipped = false 이런걸 받아올꺼다)
            {
                if (Item.itemCnt == 10) return false;
                 _item[Item.itemCnt] = item;  // 0개인 경우 0번 인덱스 , 1개인 경우 1번인덱스
                Item.itemCnt++;
                return true;
            }
        }

    }
}














