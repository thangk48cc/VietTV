﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietTV.Services
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}
