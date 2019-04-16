using System;
using System.IO;
using System.Timers;

namespace 迷宮地圖製作
{
    class Program
    {

        /*------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*----------------------------------------------------make by 103111237 袁瑞彤 ---------------------------------------------------*/
        /*------------------------------------------------------------------------------------------------------------------------------------------------------*/

        // var
        public static int x = 1,
                                    y = 1,
                                    move_y = 7,
                                    cont_s = 0,
                                    cont_e = 0,
                                    save_s_x = 0,
                                    save_s_y = 0,
                                    save_e_x = 0,
                                    save_e_y = 0,
                                    start_pot_x = 0,
                                    start_pot_y = 0,
                                    end_pot_x = 0,
                                    end_pot_y = 0,
                                    time_min = 0,
                                    time_sec = 1,
                                    borker_cont = 10,
                                    borker_use = 0,
                                    loadfile_use = 0;

        
        public static bool rightmap_use = false,
                                       endgame = false,
                                       testingcode = false,/*testing code*/
                                       giveanswer = false;  
        public static Timer t1 = new Timer(10000);
        public static Timer t2 = new Timer(10000);
        public static Random rnd = new Random();
        public static write wr = new write();
        public static ConsoleKeyInfo cki;
        public static int[,] borker_pot_new = new int[5, 2];
        public static string[] borker_pot_save = new string[5];
        public static string[,] map = new string[23, 61];
        public static string[,] rightmap = new string[23, 61];
        public static string filename = "", username = "",fileload = "";


        //show the menu,
        static void Main(string[] args)
        {
            fileload = System.IO.Directory.GetCurrentDirectory();
            string[] title = { "載入地圖", "製作地圖", "顯示排行", "結束遊戲" };
            int[,] num_pot = new int[4, 2]{{35,7},
                                                                  {35,9},
                                                                  {35,11},
                                                                  {35,13}};
            clearvalue();
            wr.writer("您的大名 : ", 25, 7);
            username = Console.ReadLine();
            if (username == "")
                username = "noname";
            Console.Clear();
            
            while (true)
            {
                filename = "";
                Console.ForegroundColor = ConsoleColor.Magenta;
                wr.writer(username + " 您好!!", 35, 3);
                Console.ForegroundColor = ConsoleColor.White;
                
                for (int i = 0; i < 4; i++)
                {
                    if (move_y == num_pot[i, 1])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        wr.writer(title[i], num_pot[i, 0], num_pot[i, 1]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        wr.writer(title[i], num_pot[i, 0], num_pot[i, 1]);
                    }
                }

                cki = Console.ReadKey(true);
                
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        move_y = move_y <= 7 ? move_y = 7 : move_y - 2;
                        break;
                    case ConsoleKey.DownArrow:
                        move_y = move_y >= 13 ? move_y = 13 : move_y + 2;
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        switch (move_y)
                        {
                            case 7:
                                loadmap();
                                break;
                            case 9:
                                maprule();
                                makemap();
                                break;
                            case 11:
                                showlist();
                                break;
                            case 13:
                                Environment.Exit(0);
                                break;
                        }
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
        }



        /*--------------------------for the game--------------------------*/

        //reset value
        static void clearvalue()
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Clear();
            Console.CursorVisible = false;
            cont_s = 0;
            cont_e = 0;
            save_s_x = 0;
            save_s_y = 0;
            save_e_x = 0;
            save_e_y = 0;
            start_pot_x = 0;
            start_pot_y = 0;
            end_pot_x = 0;
            end_pot_y = 0;
            borker_cont = 0;
          
        }

        //show the record
        static void showlist()
        {
            showfile();
            Console.Clear();
            wr.writer("地圖名稱 : " + filename, 22, 7);
            wr.writer("--------曾經的紀錄----------- ", 22, 9);
            loadlist();
            wr.writer("按下Enter返回主畫面", 22, 11 + loadfile_use);
            Console.ReadLine();
            Console.Clear();
        }

        //connect the gamestart ane gameover
        static void loadmap()
        {
            showfile();
            inputmap();
            Console.Clear();
            gamerule();
            showmap();
            gamestart();
            Console.Clear();
            return;
        }

        //show the gamerule
        static void gamerule()
        {
            wr.writer("------規則------", 25, 7);
            wr.writer("1.請按WASD進行上下左右移動", 20, 8);
            wr.writer("2.*號為牆壁,不可穿越", 20, 9);
           wr. writer("3.此遊戲不是限時遊戲,但設置有計時器可自行利用", 20, 10);
           wr. writer("4.每分鐘會隨機產生道具,吃道具後請小心行徑,可破牆", 20, 11);
           wr. writer("5.按下enter後開始遊戲", 20, 12);
           wr. writer("5.按下Ese後結束遊戲", 20, 12);
            Console.ReadLine();
            Console.Clear();
        }

        //show all file
        static void showfile()
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data")))
            {
                Directory.CreateDirectory(@"C:\data");
                if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\test")))
                {
                    Directory.CreateDirectory(@"C:\data\test");
                }
                if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\test\test.txt")))
                {
                    File.Copy(fileload + @"\test\test.txt", @"C:\data\test\test.txt");
                }
                if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\test\rightmap.txt")))
                {
                    File.Copy(fileload + @"\test\rightmap.txt", @"C:\data\test\rightmap.txt");
                }         
            }
            string[] allfile = Directory.GetDirectories(@"C:\data", "*.*");
            Console.WriteLine("----------檔案目錄--------------");
            foreach (string s in allfile)
            {
                System.IO.FileInfo fi = null;
                try
                {
                    fi = new System.IO.FileInfo(s);
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                Console.WriteLine("--------------------------------");
                Console.WriteLine("地圖名稱 : {0}", fi.Name);
                Console.WriteLine("--------------------------------");
            }
            Console.WriteLine("--------------------------------");
            Console.Write("請輸入您想開啟的地圖 : ");
            filename = Console.ReadLine();
            checkname_load();
        }

        //check file's exist
        static void checkname_load()
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\" + filename + ".txt")))
            {
                Console.WriteLine("{0}不存在,是否要製作新地圖 Y/N :", filename);
                cki = Console.ReadKey();
                Console.Clear();
                switch (cki.Key)
                {
                    case ConsoleKey.Y:
                        makemap();
                        break;
                    case ConsoleKey.N:
                        showfile();
                        break;
                }
            }
        }

        //input the file
        static void inputmap()
        {
            FileStream fs = new FileStream(@"C:\data\" + filename + @"\" + filename + ".txt", FileMode.Open);
            StreamReader re = new StreamReader(fs);
            string input_str;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                input_str = re.ReadLine();
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = input_str[j].ToString();
                    if (map[i, j] == "S")
                    {
                        start_pot_x = j;
                        start_pot_y = i;
                    }
                    if (map[i, j] == "E")
                    {
                        end_pot_x = j;
                        end_pot_y = i;
                    }
                }
            }
            re.Close();
        }

        //start the game
        static void gamestart()
        {
            checkrightmap();
            t1.Elapsed += new ElapsedEventHandler(timer);
            t1.Interval = 1000;
            t1.Enabled = true;
            t2.Elapsed += new ElapsedEventHandler(borker);
            t2.Interval = 10000;
            t2.Enabled = true;
            findstartend();
            while (true)
            {
                if (x == end_pot_x && y == end_pot_y)
                {
                    break;
                }
                if (rightmap_use)
                {
                    wr.writer("按E公佈解答", 61, 5);
                }
                else
                {
                    wr.writer("此地圖沒有解答", 61, 5);
                }
                wr.writer("破牆次數 : " + borker_cont.ToString("00"), 61, 3);
                check_borker();
                Console.SetCursorPosition(x, y);
                Console.Write("@");
                cki = Console.ReadKey(true);
                Console.SetCursorPosition(x, y);
                Console.Write(" ");
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (borker_cont == 0)
                        {
                            y = y <= 1 ? y = 1 : map[y - 1, x] == "*" ? y += 0 : y - 1;
                        }
                        else if (borker_cont != 0)
                        {
                            y = y <= 1 ? y = 1 : y - 1;
                            if (map[y, x] == "*")
                            {
                                map[y, x] = " ";
                                borker_cont--;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (borker_cont == 0)
                        {
                            y = y >= 21 ? y = 21 : map[y + 1, x] == "*" ? y += 0 : y + 1;
                        }
                        else if (borker_cont != 0)
                        {
                            y = y >= 21 ? y = 21 : y + 1;
                            if (map[y, x] == "*")
                            {
                                map[y, x] = " ";
                                borker_cont--;
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (borker_cont == 0)
                        {
                            x = x <= 1 ? x = 1 : map[y, x - 1] == "*" ? x += 0 : x - 1;
                        }
                        else if (borker_cont != 0)
                        {
                            x = x <= 1 ? x = 1 : x - 1;
                            if (map[y, x] == "*")
                            {
                                map[y, x] = " ";
                                borker_cont--;
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (borker_cont == 0)
                        {
                            x = x >= 59 ? x = 59 : map[y, x + 1] == "*" ? x += 0 : x + 1;
                        }
                        else if (borker_cont != 0)
                        {
                            x = x >= 59 ? x = 59 : x + 1;
                            if (map[y, x] == "*")
                            {
                                map[y, x] = " ";
                                borker_cont--;
                            }
                        }
                        break;
                    case ConsoleKey.E:
                        if (rightmap_use)
                        {
                            endgame = false;
                            closetimer();
                            showrightroad();
                            gameover();
                            return;
                        }
                        break;
                    case ConsoleKey.Escape:
                        endgame = false;
                        gameover();
                        return;
                }
            }
            endgame = true;
            gameover();
        }

        //show the answer 
        static void showrightroad()
        {
            clearvalue();
            Console.Clear();
            showmap();
            loadrightmap();
            for (int i = 0; i < rightmap.GetLength(0); i++)
            {
                for (int j = 0; j < rightmap.GetLength(1); j++)
                {
                    if (rightmap[i, j] == "O")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        wr.writer("O", j, i);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            wr.writer("已顯示解答,按下Enter結束遊戲", 0, 23);
            Console.ReadLine();
            Console.Clear();
        }

        //close the timer
        static void closetimer()
        {
            t2.Enabled = false;
            t1.Enabled = false;
        }

        //show user's time and ask for continue
        static void gameover()
        {
            closetimer();
            Console.Clear();
            if (endgame)
            {
                wr.writer("遊戲結束!!成功抵達終點!!!", 22, 7);
                wr.writer(username + "您總共花了 " + time_min.ToString("00") + "分" + time_sec.ToString("00") + "秒 ", 22, 8);
                wr.writer(filename + "曾經的紀錄 : ", 22, 9);
                loadlist();
               wr. writer("這次的遊玩紀錄是否登入排行榜中 ? Y/N ", 22, 11 + loadfile_use);
                cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.Y:
                        savelist();
                        break;
                    case ConsoleKey.N:
                        break;
                }
                Console.Clear();
                wr.writer("按Enter返回主畫面....", 22, 9);
                Console.ReadLine();
            }
            else
            {
               wr. writer("遊戲結束!!無法抵達終點!!!", 22, 7);
               wr. writer(filename + "曾經的紀錄 : ", 22, 9);
                loadlist();
              wr.  writer("按下Enter返回主畫面", 22, 11 + loadfile_use);
                Console.ReadLine();
            }
        }

        //check get props or not
        static void check_borker()
        {
            for (int i = 0; i < 5; i++)
            {
                if (x == borker_pot_new[i, 0] && y == borker_pot_new[i, 1])
                {
                    borker_cont++;
                    borker_pot_new[i, 0] = 0;
                    borker_pot_new[i, 1] = 23;
                }
            }
        }

        //read the record
        static void loadlist()
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\" + filename + "list.txt")))
            {
                File.WriteAllText(@"c:\data\" + filename + @"\" + filename + "list.txt", "");
            }
            string[] list = File.ReadAllLines(@"c:\data\" + filename + @"\" + filename + "list.txt");
            if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\" + filename + "time.txt")))
            {
                File.WriteAllText(@"c:\data\" + filename + @"\" + filename + "time.txt", "");
            }
            string[] time = File.ReadAllLines(@"c:\data\" + filename + @"\" + filename + "time.txt");
            loadfile_use = time.Length;
            if (list.Length == 0)
            {
                wr.writer("暫無紀錄", 22, 10 + loadfile_use);
            }
            else
            {
                for (int i = 0; i < list.Length; i++)
                {
                    wr.writer("玩家 : " + list[i], 22, 10 + i);
                    wr.writer("時間 :  " + (Convert.ToInt32(time[i]) / 60).ToString("00") + ":" + (Convert.ToInt32(time[i]) - (Convert.ToInt32(time[i]) / 60)).ToString(), 35, 10 + i);
                }
            }
        }

        //reaad the answer record
        static void loadrightmap()
        {
            FileStream fs = new FileStream(@"C:\data\" + filename + @"\rightmap.txt", FileMode.Open);
            StreamReader re = new StreamReader(fs);
            string input_str;
            for (int i = 0; i < rightmap.GetLength(0); i++)
            {
                input_str = re.ReadLine();
                for (int j = 0; j < rightmap.GetLength(1); j++)
                {
                    rightmap[i, j] = input_str[j].ToString();
                }
            }
            re.Close();
        }

        //save the record 
        static void savelist()
        {

            if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\" + filename + "list.txt")))
            {
                File.WriteAllText(@"c:\data\" + filename + @"\" + filename + "list.txt", "");
            }
            string[] list = File.ReadAllLines(@"c:\data\" + filename + @"\" + filename + "list.txt");
            if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\" + filename + "time.txt")))
            {
                File.WriteAllText(@"c:\data\" + filename + @"\" + filename + "list.txt", "");
            }
            string[] time = File.ReadAllLines(@"c:\data\" + filename + @"\" + filename + "time.txt");
            int[] num_time = new int[list.Length];
            string[] save_list = new string[list.Length + 1];
            string[] save_time = new string[time.Length + 1];
            int temp_num;
            string temp_str;
            for (int i = 0; i < list.Length; i++)
            {
                save_list[i] = list[i];
                save_time[i] = time[i];
            }
            save_list[list.Length] = username;
            save_time[list.Length] = (time_min * 60 + time_sec).ToString();
            for (int i = 0; i < save_time.Length - 1; i++)
            {
                num_time[i] = Convert.ToInt32(save_time[i]);
            }
            for (int i = 1; i <= num_time.Length - 1; i++)
            {
                for (int j = 1; j <= num_time.Length - i; j++)
                {
                    if (num_time[j] > num_time[j - 1])
                    {
                        temp_num = num_time[j - 1];
                        num_time[j - 1] = num_time[j];
                        num_time[j] = temp_num;
                        temp_str = list[j - 1];
                        save_list[j - 1] = save_list[j];
                        save_list[j] = temp_str;
                    }
                }
            }
            for (int i = 0; i < list.Length; i++)
            {
                save_time[i] = num_time[i].ToString();
            }

            File.WriteAllLines(@"c:\data\" + filename + @"\" + filename + "time.txt", save_time);
            File.WriteAllLines(@"c:\data\" + filename + @"\" + filename + "list.txt", save_list);
            wr.writer("您的紀錄已保存!!!", 22, 12 + loadfile_use); Console.ReadLine();
        }

        //check answer exist
        static void checkrightmap()
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\rightmap.txt")))
            {
                rightmap_use = false;
            }
            else if (System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\rightmap.txt")))
            {
                rightmap_use = true;
            }
        }
        /*---------------------------------------------------------------------------------*/



        /*--------------------------for making map--------------------------*/

        //for make map using
        static void makemap()
        {
            resetmap();
            showmap();
            while (true)
            {
                wr.writer("@", x, y);
                cki = Console.ReadKey(true);
                wr.writer(map[y, x], x, y);
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        y = y <= 1 ? y = 1 : y - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        y = y >= 21 ? y = 21 : y + 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        x = x <= 1 ? x = 1 : x - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        x = x >= 59 ? x = 59 : x + 1;
                        break;
                    case ConsoleKey.Spacebar:
                        map[y, x] = " ";
                        x = x - 1 < 1 ? x = 1 : x + 0;
                        wr.writer(map[y, x], x, y);
                        break;
                    case ConsoleKey.S:
                        map[y, x] = "S";
                        x = x - 1 < 1 ? x = 1 : x + 0;
                        wr.writer(map[y, x], x, y);
                        if (cont_s != 0)
                        {
                            map[save_s_x, save_s_y] = "*";
                            wr.writer(map[save_s_x, save_s_y], save_s_y, save_s_x);
                        }
                        save_s_x = y;
                        save_s_y = x;
                        cont_s = 1;
                        break;
                    case ConsoleKey.Delete:
                         map[y, x] = "*";
                        x = x - 1 < 1 ? x = 1 : x + 0;
                        wr.writer(map[y, x], x, y);
                        break;
                    case ConsoleKey.E:
                        map[y, x] = "E";
                        x = x - 1 < 1 ? x = 1 : x + 0;
                        wr.writer(map[y, x], x, y);
                        if (cont_e != 0)
                        {
                            map[save_e_x, save_e_y] = "*";
                            wr.writer(map[save_e_x, save_e_y], save_e_y, save_e_x);
                        }
                        save_e_x = y;
                        save_e_y = x;
                        cont_e = 1;
                        break;
                    case ConsoleKey.Escape:
                        if (cont_s == 0)
                        {
                            checkfb("沒有起點");
                        }
                        else if (cont_e == 0)
                        {

                            checkfb("沒有終點");
                        }
                        else if (checkstart())
                        {
                            checkfb("沒有道路連接到起點");
                        }
                        else if (checkend())
                        {
                            checkfb("沒有道路連接到終點");
                        }
                        else
                        {
                            Console.Clear();
                            rightrode();
                            givefilename("請輸入此迷宮您想要取的名字 : ");
                            checkname_save();
                            if(giveanswer)
                            {
                                outputmap();
                                outputrightmap();
                            }
                            else
                            {
                                outputmap();
                            }
                            
                            Console.Clear();
                            return;
                        }
                        break;
                }
            }
        }

        //show the rule
        static void maprule()
        {
            wr.writer("-----規則------", 22, 7);
            wr.writer("1.按下空白鍵設置路徑", 20, 8);
            wr.writer("2.按下S設置起點", 20, 9);
            wr.writer("3.按下E設置終點", 20, 10);
            wr.writer("4.製作完成後請按ESC進行地圖命名", 20, 11);
            wr.writer("5.按下enter後開始製作地圖", 20, 12);
            Console.ReadLine();
            Console.Clear();
        }

        //ask for the answer
        static void rightrode()
        {
            while (true)
            {
                wr.writer("是否提供正確解答? Y/N", 25, 7);
                cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.Y:
                        Console.Clear();
                        giveanswer = true;
                        drawrightrod();
                       
                        return;
                    case ConsoleKey.N:
                        return;
                }
            }
        }

        //answer's rule
        static void rightroderule()
        {
            wr.writer("-----規則------", 22, 7);
            wr.writer("1.按下空白鍵設置路徑", 20, 8);
            wr.writer("2.製作完成後請按ESC進行地圖命名", 20, 9);
            wr.writer("3.按下Delete刪除以繪製的路徑點", 20, 8);
            wr.writer("4.按下enter後開始製作地圖", 20, 12);
            Console.ReadLine();
            Console.Clear();
        }

        //make the answer
        static void drawrightrod()
        {
            findstartend();
            resetrghtmap();
            showmap();
            while (true)
            {
                wr.writer("@", x, y);
                cki = Console.ReadKey(true);
                Console.ForegroundColor = ConsoleColor.Blue;
                wr.writer(rightmap[y, x], x, y);
                Console.ForegroundColor = ConsoleColor.White;
                if (map[y, x] == "S")
                    wr.writer(map[y, x], x, y);
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        y = y <= 1 ? y = 1 : (map[y - 1, x] == "*" || map[y - 1, x] == "S" || map[y - 1, x] == "E") ? y += 0 : y - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        y = y >= 21 ? y = 21 : (map[y + 1, x] == "*" || map[y + 1, x] == "S" || map[y + 1, x] == "E") ? y += 0 : y + 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        x = x <= 1 ? x = 1 : (map[y, x - 1] == "*" || map[y, x - 1] == "S" || map[y, x - 1] == "E") ? x -= 0 : x - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        x = x >= 59 ? x = 59 : (map[y, x + 1] == "*" || map[y, x + 1] == "S" || map[y, x + 1] == "E") ? x += 0 : x + 1;
                        break;
                    case ConsoleKey.Spacebar:
                        rightmap[y, x] = "O";
                        x = x - 1 < 1 ? x = 1 : x + 0;
                        wr.writer("O", x, y);
                        break;
                    case ConsoleKey.Delete:
                        rightmap[y, x] = " ";
                        x = x - 1 < 1 ? x = 1 : x + 0;
                        wr.writer(rightmap[y, x], x, y);
                        break;
                    case ConsoleKey.Escape:
                        if (checkrightstart())
                        {
                            checkfb("沒有道路連接到起點");
                        }
                        else if (checkrightend())
                        {
                            checkfb("沒有道路連接到終點");
                        }
                        else
                        {

                            outputrightmap();
                            Console.Clear();
                            return;
                        }
                        break;
                }
            }
        }

        //show the feedback
        static void checkfb(string str_fb)
        {
            Console.Clear();
            wr.writer(str_fb, 35, 7);
            Console.ReadLine();
            Console.Clear();
            showmap();
        }

        //check the pot_end connet the rode or not (for make map)
        static bool checkend()
        {
            if (map[save_e_x + 1, save_e_y] == "*" && map[save_e_x - 1, save_e_y] == "*" && map[save_e_x, save_e_y + 1] == "*" && map[save_e_x, save_e_y - 1] == "*")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //check the pot_start connet the rode or not(for make map)
        static bool checkstart()
        {
            if (map[save_s_x + 1, save_s_y] == "*" && map[save_s_x - 1, save_s_y] == "*" && map[save_s_x, save_s_y + 1] == "*" && map[save_s_x, save_s_y - 1] == "*")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //check the answer pot_end connet the rode or not (for answer)
        static bool checkrightend()
        {
            if (map[save_e_x + 1, save_e_y] == " " && map[save_e_x - 1, save_e_y] == " " && map[save_e_x, save_e_y + 1] == " " && map[save_e_x, save_e_y - 1] == " ")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //check the answer pot_start connet the rode or not(for answer)
        static bool checkrightstart()
        {
            if (map[save_s_x + 1, save_s_y] == " " && map[save_s_x - 1, save_s_y] == " " && map[save_s_x, save_s_y + 1] == " " && map[save_s_x, save_s_y - 1] == " ")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //ask for the filename 
        static void givefilename(string str_name)
        {
            Console.Clear();
            Console.Write(str_name);
            filename = Console.ReadLine();
            if (filename == "")
                filename = "noname";
        }

        //check the filename exist or not
        static void checkname_save()
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(@"C:\data\" + filename + @"\" + filename + ".txt")))
            {
                Console.WriteLine("該檔名已存在,是否要蓋過此檔案 Y/N :");
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.Y:
                        return;
                    case ConsoleKey.N:
                        givefilename("請輸入此迷宮您想要取的名字 : ");
                        break;
                }
            }
        }

        //reset the map
        static void resetmap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = "*";
                }
            }
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[0, j] = "#";
                    map[22, j] = "#";
                }
                map[i, 0] = "#";
                map[i, 60] = "#";
            }
        }

        //reset the answer
        static void resetrghtmap()
        {
            for (int i = 0; i < rightmap.GetLength(0); i++)
            {
                for (int j = 0; j < rightmap.GetLength(1); j++)
                {
                    rightmap[i, j] = " ";
                }
            }
        }

        // output the map
        static void outputmap()
        {
            Directory.CreateDirectory(@"C:\data\" + filename);
            FileStream fs = new FileStream(@"C:\data\" + filename + @"\" + filename + ".txt", FileMode.Create);
            StreamWriter write = new StreamWriter(fs);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    write.Write(map[i, j]);
                }
                write.WriteLine();
            }
            write.Close();
        }

        //output the answer
        static void outputrightmap()
        {
            FileStream fs = new FileStream(@"C:\data\" + filename + @"\rightmap.txt", FileMode.Create);
            StreamWriter write = new StreamWriter(fs);
            for (int i = 0; i < rightmap.GetLength(0); i++)
            {
                for (int j = 0; j < rightmap.GetLength(1); j++)
                {
                    write.Write(rightmap[i, j]);
                }
                write.WriteLine();
            }
            write.Close();
        }

        //show the map
        static void showmap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
        /*---------------------------------------------------------------------------------*/




        /*--------------------------for the timer--------------------------*/

        //for the time
        static void timer(object source, ElapsedEventArgs e)
        {
            time_sec++;
            if (time_sec >= 60)
            {
                time_sec = 1;
                time_min++;
            }
            wr.writer("經過時間 : " + time_min.ToString("00") + "分" + time_sec.ToString("00") + "秒", 61, 1);
        }

        //for the props
        static void borker(object source, ElapsedEventArgs e)
        {
            if (borker_use != 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    map[borker_pot_new[i, 1], borker_pot_new[i, 0]] = borker_pot_save[i];
                    Console.ForegroundColor = ConsoleColor.White;
                    wr.writer(borker_pot_save[i], borker_pot_new[i, 0], borker_pot_new[i, 1]);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                borker_pot_new[i, 1] = 0;
                borker_pot_new[i, 1] = 0;
            }
            borker_use = 1;
            for (int i = 0; i < 5; i++)
            {
                borker_pot_new[i, 0] = rnd.Next(2, 57);
                borker_pot_new[i, 1] = rnd.Next(2, 21);
            }
            for (int i = 0; i < 5; i++)
            {
                borker_pot_save[i] = map[borker_pot_new[i, 1], borker_pot_new[i, 0]];
                map[borker_pot_new[i, 1], borker_pot_new[i, 0]] = "#";
                Console.ForegroundColor = ConsoleColor.Black;
                wr.writer("#", borker_pot_new[i, 0], borker_pot_new[i, 1]);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        //find the pot_start
        static void findstartend()
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == "S")
                    {
                        x = j;
                        y = i;
                    }
                }

        }
        /*---------------------------------------------------------------------------*/

        
    }
}