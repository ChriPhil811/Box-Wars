using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Box_Wars.Classes
{
    class Hero
    {
        public int x, y, size;

        public Hero(int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
        }

        public void MoveUpDown(int speed)
        {
            y += speed;
        }

        public void MoveRightLeft(int speed)
        {
            x += speed;
        }

        public void Stop(string direction)
        {
            switch(direction)
            {
                case "w":
                    y += 5;
                    break;
                case "a":
                    x += 5;
                    break;
                case "s":
                    y -= 5;
                    break;
                case "d":
                    x -= 5;
                    break;
            }
        }
    }
}
