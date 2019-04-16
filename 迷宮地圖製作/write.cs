using System;

namespace 迷宮地圖製作
{
    class write
    {
        public void writer(string str ,int x,int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(str);
        }
    }
}
