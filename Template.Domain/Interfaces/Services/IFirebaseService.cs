﻿using FirebaseAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Interfaces.Services
{
    public interface IFirebaseService
    {
        FirebaseApp GetFirebaseApp();
        string GetProjectId();
    }
}
