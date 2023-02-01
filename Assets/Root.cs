using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Root
    {
        public List<Swarm> swarm;

        public Root(List<Swarm> robot) {
            this.swarm = robot;
        }
    }