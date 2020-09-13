﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ARKBreedingStats.Library;
using ARKBreedingStats.species;

namespace ARKBreedingStats.uiControls
{
    public partial class ParentInheritance : UserControl
    {
        private readonly List<int[]>[] _lines;

        public ParentInheritance()
        {
            InitializeComponent();
            _lines = new[] { new List<int[]>(), null };
            Paint += ParentStats_Paint;
        }

        private void ParentStats_Paint(object sender, PaintEventArgs e)
        {
            if (ControlOffspring.Creature != null && _lines[0].Any())
                Pedigree.DrawLines(e.Graphics, _lines);
        }

        internal void SetCreatures(Creature offspring = null, Creature mother = null, Creature father = null)
        {
            if (offspring == null && mother == null && father == null)
            {
                Visible = false;
                return;
            }
            Visible = true;

            var enabledColorRegions = offspring?.Species?.EnabledColorRegions;
            if (enabledColorRegions != null)
            {
                ControlOffspring.enabledColorRegions = enabledColorRegions;
                ControlMother.enabledColorRegions = enabledColorRegions;
                ControlFather.enabledColorRegions = enabledColorRegions;
            }

            SetCreature(ControlOffspring, offspring);
            SetCreature(ControlMother, mother);
            SetCreature(ControlFather, father);

            void SetCreature(PedigreeCreature pc, Creature c)
            {
                if (c == null)
                    pc.Visible = false;
                else
                {
                    pc.Creature = c;
                    pc.Visible = true;
                }
            }

            _lines[0].Clear();
            if (offspring != null && (mother != null || father != null))
                Pedigree.CreateGeneInheritanceLines(offspring, mother, father, _lines, 6, 60);

            Invalidate();
        }

        public void SetLocalizations()
        {
            GbParents.Text = Loc.S("Parents");
        }

        internal void SetSpecies(Species species)
        {
            pedigreeCreatureHeaders.SetCustomStatNames(species?.statNames);
        }
    }
}