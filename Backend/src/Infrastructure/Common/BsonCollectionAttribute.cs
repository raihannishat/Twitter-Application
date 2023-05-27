namespace Infrastructure.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; private set; }

    public BsonCollectionAttribute(string collectionName) => CollectionName = collectionName;
}
