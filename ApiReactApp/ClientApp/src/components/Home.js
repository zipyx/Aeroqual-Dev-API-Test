import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1>Aeroqual Technical Test</h1>
                <p>The solution contains five Projects:</p>
                <ul>
                    <li>ApiAWS</li>
                    <li>ApiLibrary</li>
                    <li>ApiReactApplication (This application running)</li>
                    <li>ApiTest</li>
                    <li>ApiUnitTest</li>
                </ul>
                <p>A few design thoughts:</p>
                <ul>
                    <li><strong>MVC Repository Pattern with a Library</strong>.
                        I've designed the structure of the project to include an API library with a <code>repository pattern</code> for abstraction between data and the business logic coupled with MVC on an application layer.
                        This is also a little different as I've placed the repository pattern within a library only, reason being is ideally I would have used something like <em>Entity Framework</em> to migrate JSON objects into
                        a database to describe relationships between data as I would implement security but I wasn't sure what the constraints were for this test since it was specified that data must be served FROM a JSON file.
                    </li>
                    <li><strong>DBContext (Entity Framework)</strong>.
                        You'll also see that the respository code within the library in relation to it's <em>"context"</em> is incomplete, it cannot be complete as I don't have a database
                        to work with but thought it's good practice to conceptually display the idea, that I would migrated the JSON objects into a <code>relational database</code>, creating it's relationships with other inbuilt frameworks.
                    </li>
                    <li><strong>Unit Testing</strong>.
                        I've also briefly included some basic unit testing to disply that production code should always be tested and staged before release to production, they should have some form of <code>integration testing </code>
                        as well as unit and behavioural to ensure that data in motion or at rest isn't exploited.
                    </li>
                    <li><strong>Security</strong>.
                        I've added security using <code>JWT</code> instead of <code>Auth0</code> for API's authorization, this is also incomplete on purpose as this is only a test but thought it was a worthwhile to mention here as it is
                        important to secure API access for production ready use. I decided for simplistic sake, it might be better to use JWT as it provides manageable access over.
                    </li>
                    <li><strong>Other Reasons</strong>.
                        Other reasons for creating a library for this particular test is to show that standard libraries are portable and work well with many projects, there are no dependencies on the particular application
                        but an easy to access library <code>simplying the complexity</code> of a project, I'm demonstrating this by including an <code>AWS lambda</code> & <code>AWS API Gateway</code>, that is included in the solutions file.
                    </li>
                </ul>
                <p>My <code>thoughts</code>? I added the <code>code</code> highlights cause it was fun to add.</p>
            </div>
        );
    }
}
