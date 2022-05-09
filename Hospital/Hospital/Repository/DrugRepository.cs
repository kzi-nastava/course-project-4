﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.Repository
{
    class DrugRepository
    {
        private IngredientService _ingredientService;

        public DrugRepository()
        {
            _ingredientService = new IngredientService();
        }

        public List<Drug> Load()
        {
            List<Drug> allDrugs = new List<Drug>();
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Data\drugs.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string idDrug = fields[0];
                    string idName = fields[1];
                    List<string> ingredientIds = fields[2].Split(';').ToList();
                    List<Ingredient> ingredients = new List<Ingredient>();
                    foreach (string id in ingredientIds)
                    {
                        ingredients.Add(_ingredientService.Get(id));
                    }
                    Drug.StateDrug state = (Drug.StateDrug)int.Parse(fields[3]);
                    Drug newDrug = new Drug(idDrug, idName, ingredients, state);
                    allDrugs.Add(newDrug);
                }
            }
            return allDrugs;
        }
    }
}
