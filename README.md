# What is this?

A custom Sitecore JSS GraphQL query that extends the built in Content Search API query and adds the ability to pass in conditions that can be OR'ed or AND'ed when the query executes. Conditions can also be grouped.

## Examples

Basic query, filtering the same field `contenttype`, but OR'ing the conditions together.
```
{
  customSearch(
    conditions:
    	[{ name:"contenttype", value:"Employee" },
    	{ name:"contenttype", value:"Article", useor: true}]
    	facetOn:["contenttype", "category"]
  		first: 5
  		after: "0") {
    facets {
      name
      values {
        value
        count
      }
    }
    results {
      items {
        item {
          name
          path
          url
        }
      }
    }
  }
}
```

Condition grouping query
```
{
  customSearch(
    conditions:
    	[ { 
        	group: [ 
                { name:"contenttype", value:"Employee" },
      			{ name:"contenttype", value:"Article", useor: true}
          ],
      	},
      	{ name:"category", value:"History" }
      ]
    	facetOn:["contenttype", "category"]
  		first: 5
  		after: "0") {
    facets {
      name
      values {
        value
        count
      }
    }
    results {
      items {
        item {
          name
          path
          url
        }
      }
    }
  }
}
```

## Patch File

```
<?xml version="1.0" encoding="utf-8" ?>

<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/" xmlns:role="http://www.sitecore.net/xmlconfig/role/">
    <sitecore>
        <api>
            <GraphQL>
                <defaults>
                    <content>
                        <schemaProviders>
                            <systemContent>
                                <queries hint="raw:AddQuery">
                                    <query name="customSearch" type="GraphQL.SearchSchema.CustomSearchQuery, GraphQL.SearchSchema" />
                                </queries>
                            </systemContent>
                        </schemaProviders>
                    </content>
                </defaults>
            </GraphQL>
        </api>
    </sitecore>
</configuration>

```