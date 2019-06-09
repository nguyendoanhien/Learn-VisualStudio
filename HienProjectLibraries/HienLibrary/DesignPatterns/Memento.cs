using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesStore.DesignPatterns
{
    public class Memento<T> where T : ICloneable
    {
        private T StateObj { get; set; }
        public T GetState()
        {
            return StateObj;
        }
        public void SetState (T stateObj)
        {
            StateObj = (T)stateObj.Clone();
        }
    }
}