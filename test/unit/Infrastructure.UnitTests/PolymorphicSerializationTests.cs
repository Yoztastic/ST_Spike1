using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Converters;
using StorageSpike.Application.Core;

namespace Infrastructure.UnitTests;

public class PolymorphicSerializationTests
{
    [Fact]
    public void Test_For_Reciprocity()
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new PolymorphicConverter<HashingAlgoDto>(new[]
                {
                    typeof(BCryptAlgoDto), typeof(PbkDf2AlgoDto)
                }),
                new JsonStringEnumConverter()
            }
        };
        var hashedPasswordAggregateEntryDto = new HashedPasswordAggregateEntryDto
        {

            HashedPasswordEntities = new List<HashedPasswordEntityDto>
            {
                new()
                {
                    HashingAlgo = new PbkDf2AlgoDto
                    {
                        Salt = "salt"
                    }
                },
                new()
                {
                    HashingAlgo = new BCryptAlgoDto
                    {
                        EnhancedEntropy = true,
                    }
                }
            }
        };

        var jsonString = JsonSerializer.Serialize(hashedPasswordAggregateEntryDto, options);
        var objectDto = JsonSerializer.Deserialize<HashedPasswordAggregateEntryDto>(jsonString, options);

        objectDto.Should().BeEquivalentTo(hashedPasswordAggregateEntryDto);
    }

    [Fact]
    public void Test_For_UnknownType()
    {
        var options = new JsonSerializerOptions
            { Converters = { new PolymorphicConverter<HashingAlgoDto>(new[] { typeof(BCryptAlgoDto), typeof(PbkDf2AlgoDto) }) } };
        var hashedPasswordAggregateEntryDto = new HashedPasswordAggregateEntryDto
        {

            HashedPasswordEntities = new List<HashedPasswordEntityDto>
            {
                new()
                {
                    HashingAlgo = new PbkDf2AlgoDto
                    {
                        Salt = "salt"
                    }
                },
                new()
                {
                    HashingAlgo = new BCryptAlgoDto
                    {
                        EnhancedEntropy = true
                    }
                },
                new()
                {
                    HashingAlgo = new HashingAlgoDto()
                }
            }
        };

        var invalidEntities = () => JsonSerializer.Serialize(hashedPasswordAggregateEntryDto, options);
        invalidEntities.Should().Throw<JsonException>()
            .WithMessage("No serialisation support for Infrastructure.UnitTests.PolymorphicSerializationTests+HashingAlgoDto");
    }

    [Fact]
    public void Test_For_InvalidType()
    {
        var inValidKnownType = ()=>new PolymorphicConverter<HashingAlgoDto>(new[] { typeof(string) });
        inValidKnownType.Should().Throw<ArgumentException>().WithMessage("HashingAlgoDto is not assignable from String (Parameter 'knownTypes')");
    }

    public class HashingAlgoDto : ISerialisePolymorphically
    {
        public string TypeDiscriminator=>GetType().Name;
        public string AlgoType => TypeDiscriminator;
    }

    internal class BCryptAlgoDto: HashingAlgoDto
    {
        public bool EnhancedEntropy { get; set; }

    }

    internal class PbkDf2AlgoDto: HashingAlgoDto
    {
        public string Salt { get; init; } = null!;

    }

    public class HashedPasswordAggregateEntryDto
    {
        public List<HashedPasswordEntityDto> HashedPasswordEntities { get; set; }
    }

    public class HashedPasswordEntityDto
    {
        public HashingAlgoDto HashingAlgo { get; set; }
    }

}



