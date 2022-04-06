using FantasyModuleParser.Classes.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FantasyModuleParser.Classes.Comparators
{
    public class ClassFeatureByLevel : IComparer
    {
        //public int Compare(ClassFeature x, ClassFeature y)
        //{
        //    if (x.Level > y.Level)
        //    {
        //        return 1;
        //    }
        //    return -1;
        //}
        public int Compare(object x, object y)
        {
            if (x as ClassFeature == null || y as ClassFeature == null)
            {
                return 0;
            }
                if (((ClassFeature)x).Level > ((ClassFeature)y).Level)
            {
                return 1;
            }
            return -1;
        }
    }
}
