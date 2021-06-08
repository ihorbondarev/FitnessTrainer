using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTrainer.MoblieApp
{
    public interface IPath
    {
        string GetDatabasePath(string filename);
    }
}
