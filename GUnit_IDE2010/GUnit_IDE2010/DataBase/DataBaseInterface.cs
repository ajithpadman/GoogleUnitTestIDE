using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnit_IDE2010.DataBase
{
    interface DataBaseInterface
    {
         void createDataBase(string dbPath);
         GUnitDB connectToDataBase(string dbPath);
        
    }
}
