﻿using System;
using System.Collections.Generic;
using masts_dvr_tool.Connectors;
using masts_dvr_tool.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace masts_dvr_tool.Tests.Connectors
{
    [TestClass]
    public class MastConnectorTests
    {
        [TestMethod]
        public void Connect_ShouldReturnUpdatedFields_WhenRun()
        {
            Mast mast = new Mast()
            {
                Address1 = "Address One",
                VOAAddressLine2 = "Address Two",
                VOAAddressLine3 = "Address Three",
                VOAAddressLine4 = "Address Four",
                VOABAName = "SillyBA",
                VOABARefNumber = "BA Ref",
                VOACellType = "Cellular Type",
                VOACtil = "asdasd",
                VOAEastings = "1212",
                VOAEffectiveFrom = "April 2017",
                VOAM25 = "True",
                VOAMastHeight = "200m",
                VOAMastOperator = "asasd",
                VOAMastStructureType = "Structure Type",
                VOANorthings = "300",
                VOAOccupier = "Occupier",
                VOARateableValue = "300",
                VOAShared = "Shared",
                VOASharedWith = "Many",
                VOASiteArea = "300",
                VOASiteRef = "reference",
                VOASiteType = "SiteType"
            };

            DataConnector<Mast> mastsConnector = new DataConnector<Mast>();

            List<PDFField> fields = new List<PDFField>()
            {
                new PDFField()
                {
                    Name = "VOANorthings",
                    Value = "300"

                },

                 new PDFField()
                {
                    Name = "FOoo",
                    Value = ""

                }
            };

            List<PDFField> result = mastsConnector.Connect(fields, mast);
            //Preserve count
            Assert.AreEqual(fields.Count, result.Count);

            //Preserve order
            Assert.AreEqual(fields[0].Name, result[0].Name);

            string updatedResult = result.Where(x => x.Name == "VOANorthings").FirstOrDefault().Value;

            Assert.AreEqual(mast.VOANorthings, updatedResult);

        }

        [TestMethod]
        public void Connector_ShouldReturn_NonDuplicateValues()
        {
            List<PDFField> listOne = new List<PDFField>()
            {
                new PDFField
                {
                    Name = "FieldOne",
                    Value = String.Empty

                },


                 new PDFField
                {
                    Name = "FieldTwo",
                    Value = String.Empty

                },

                  new PDFField
                {
                    Name = "FieldThree",
                    Value = String.Empty

                },

                  new PDFField
                  {
                    Name = "FieldFour",
                    Value = String.Empty
                  },

                  new PDFField
                  {
                    Name = "FieldFive",
                    Value = String.Empty
                  },

            };

            List<PDFField> listTwo = new List<PDFField>()
            {
                new PDFField
                {
                    Name = "FieldOne",
                    Value = "FooOne"

                },


                 new PDFField
                {
                    Name = "FieldTwo",
                    Value = "FooTwo"

                },

                  new PDFField
                {
                    Name = "FieldThree",
                    Value = "Foo Three"

                }

            };


            List<PDFField> listThree = new List<PDFField>();

            listThree.AddRange(listOne.Except(listTwo));

        }
    }
}