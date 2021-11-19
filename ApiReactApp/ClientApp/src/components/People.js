import React, { Component, useState } from 'react';


export class People extends Component {
    static displayName = People.name;

    constructor(props) {
        super(props);
        this.state = { people: [], loading: true };
    }

    componentDidMount() {
        this.populateApiData();
    }

    static renderApiData(people) {
        return (
            <><div>
            </div><table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Age</th>
                        </tr>
                    </thead>
                    <tbody>
                        {people.filter(person => {
                            if (person.name.toLowerCase() === "") {
                                return person;
                            } else if (person.name.toLowerCase().includes('')) {
                                return person;
                            }
                        })
                            .map(person => <tr key={person.id}>
                                <td>{person.id}</td>
                                <td>{person.name}</td>
                                <td>{person.age}</td>
                            </tr>
                            )}
                    </tbody>
                </table></>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : People.renderApiData(this.state.people.people);

        return (
            <div>
                <h1 id="tabelLabel" >GET API</h1>
                <p>This component fetches data from the <code>data.json</code> file located in this project directory.</p>
                <p>You can run API calls on this page to update the <code>data.json</code> file, such as:</p>
                <ul>
                    <li><code>/people</code> : <strong>GET</strong> Return every Person.</li>
                    <li><code>/people/id</code> : <strong>GET Person</strong> Return a single Person.</li>
                    <li><code>/people</code> : <strong>POST</strong> Add Person.</li>
                    <li><code>/people/id</code> : <strong>PUT</strong> Update Person record.</li>
                    <li><code>/people/id</code> : <strong>DELETE</strong> Delete Person record.</li>
                </ul>
                <p>Every time you make a request, you will need to return to the home page by simply editing the <code>URL string</code> then click back onto the <code>API UI</code> page to see these cool effects. Not enough dev time :( </p>
                {contents}
            </div>
        );
    }


    async populateApiData() {
        const response = await fetch('people');
        const data = await response.json();
        this.setState({ people: data, loading: false });
        console.log(data);
    }
}