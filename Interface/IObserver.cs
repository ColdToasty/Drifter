using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drifter.Class;
using Drifter.Class.GameObjectClass;
using Drifter.Class.Tools;


namespace Drifter.Interface
{
    internal interface IObserver
    {

        public void Update();

        public void Update(Player player);


    }
}
