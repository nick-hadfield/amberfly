Configuration example...

```
ObjectFactory.Configure((o) => {
    o.Add<ISettings, Settings>(Instancing.Singleton);
    o.Add(typeof(IRepository<>), typeof(Repository<>));
    o.AddInterfacesAndMatchingImplementations();
    o.AddImplementationsSupportingOpenGeneric(typeof(IHandler<>));
    o.AddImplementationsSupportingOpenGeneric(typeof(IHandler<,>));
});
```

Usage example...

```
string connectionString = ObjectFactory.Get<ISetting>().ConnectionString;
ObjectFactory.Get<IRepository<Customer>>().Update(customer);
```