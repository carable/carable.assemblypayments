﻿using Newtonsoft.Json;
using Xunit;
using PromisePayDotNet.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PromisePayDotNet.Abstractions;
using PromisePayDotNet.Dto;
using PromisePayDotNet.Enums;

namespace PromisePayDotNet.Tests
{
    public class DynamicFeeTest : AbstractTest
    {
        [Fact]
        public void FeeDeserialization()
        {
            const string jsonStr = "{ \"id\": \"58e15f18-500e-4cdc-90ca-65e1f1dce565\", \"created_at\": \"2014-12-29T08:31:42.168Z\", \"updated_at\": \"2014-12-29T08:31:42.168Z\", \"name\": \"Buyer Fee @ 10%\", \"fee_type_id\": 2, \"amount\": 1000, \"cap\": null, \"min\": null, \"max\": null, \"to\": \"buyer\", \"links\": { \"self\": \"/fees/58e15f18-500e-4cdc-90ca-65e1f1dce565\" } }";
            var fee = JsonConvert.DeserializeObject<Fee>(jsonStr);
            Assert.NotNull(fee);
            Assert.Equal("58e15f18-500e-4cdc-90ca-65e1f1dce565", (string)fee.Id);
            Assert.Equal("Buyer Fee @ 10%", (string)fee.Name);
        }

        [Fact]
        public void CreateFeeSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/fees_create.json");
            var client = GetMockClient(content);

            var repo = Get<IFeeRepository>(client.Object);
            var feeId = Guid.NewGuid().ToString();
            var createdFee = repo.CreateFee(new Fee
            {
                Id = feeId,
                Amount = 1000,
                Name = "Test fee #1",
                FeeType = (FeeType)1,
                Cap = "1",
                Max = "3",
                Min = "2",
                To = ReceiverOfFee.Buyer
            });
            Assert.NotNull(createdFee);
            Assert.Equal(ReceiverOfFee.Buyer, createdFee.To);
            Assert.Equal(FeeType.Fixed, createdFee.FeeType);
        }

        [Fact]
        public void CreateFeeWrongTo()
        {
            var client = GetMockClient("");

            var repo = Get<IFeeRepository>(client.Object);
            var feeId = Guid.NewGuid().ToString();
            Assert.Throws<ValidationException>(() => repo.CreateFee(new Fee
            {
                Id = feeId,
                Amount = 1000,
                Name = "Test fee #1",
                FeeType = (FeeType)1,
                Cap = "1",
                Max = "3",
                Min = "2"
                //To = None
            }));
        }

        [Fact]
        public void GetFeeByIdSuccessfull()
        {
            var content = Files.ReadAllText("./Fixtures/fees_get_by_id.json");
            var client = GetMockClient(content);

            var repo = Get<IFeeRepository>(client.Object);
            const string id = "79116c9f-d750-4faa-85c7-b7da36f23b38";
            var fee = repo.GetFeeById(id);
            Assert.Equal(id, (string)fee.Id);
        }

        [Fact]
        public void ListFeeSuccessfully()
        {
            var content = Files.ReadAllText("./Fixtures/fees_list.json");
            var client = GetMockClient(content);

            var repo = Get<IFeeRepository>(client.Object);
            var fees = repo.ListFees();
            Assert.NotNull(fees);
            Assert.True(fees.Any());
        }
    }
}
