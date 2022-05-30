using System;
using System.Collections.Generic;

namespace App.MtgService {
    class Deck {
        public string Name { get; set; }
        public List<Card> Cards {  get { return cards; } }
        public int Size() { return cards.Count; }
        public bool IsCommander {  get { return Size() == 100; } }
        public bool IsPioneer {  get { return Size() == 60; } }
        public bool IsStandard { get { return Size() == 60; } }// && Cards.(c => c.IsStandard); } }

        private List<Card> cards = new List<Card>();
    }
}
