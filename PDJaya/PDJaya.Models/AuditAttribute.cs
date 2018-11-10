﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PDJaya.Models
{
    public class AuditAttribute
    {
        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
        public DateTime Created { set; get; }
        public DateTime Updated { set; get; }


    }
}
