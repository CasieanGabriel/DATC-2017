import React from 'react';


class AppPage extends React.Component{
    constructor(props){
        super(props);//props.appProps
        this.state=props;
    }

    render(){
        return(
            <div>
                <p>react !!!</p>
                <table className="table">
                    <tbody>
                        <tr>
                            <th>Date</th>
                            <th>Temperature</th>
                            <th>Humidity</th>
                            <th>Pressure</th>
                        </tr>

                        <tr>
                            <td>{this.props.appProps.date}</td>
                            <td>{this.props.appProps.temperature}</td>
                            <td>{this.props.appProps.humidity}</td>
                            <td>{this.props.appProps.pressure}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        )
    }
}


export default AppPage;