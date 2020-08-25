﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USF_Health_MVC_EF.Models
{
    public class IndividualStudies
    {

        Individual individual { get; set; }
        public List<Individual> individuals { get; set; }
        public List<SpPools> SpIndividuals { get; set; }
        public List<SpStudies> studies { get; set; }

    }
}
