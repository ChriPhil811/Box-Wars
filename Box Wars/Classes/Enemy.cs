using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Box_Wars.Classes
{
    class Enemy
    {
        public int x, y, size;
        
        public Enemy(int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
        }

        public void MoveEnemyUpDown(int speed)
        {
            y += speed;
        }

        public void MoveEnemyLeftRight(int speed)
        {
            x += speed;
        }
    }
}
