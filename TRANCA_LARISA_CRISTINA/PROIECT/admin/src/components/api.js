var api = {
  getResource(){
    var url = 'http://api.wunderground.com/api/7371ed5d87903525/forecast/q/zmw:00000.74.15247.json';
    return fetch(url).then((res) => res.json());
  },
  postResource(params){
    return fetch("http://www.example.co.uk/login", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(params)
    })

        .then((response) => response.json())
        .then((responseData) => {
            console.log(
                "POST Response",
                "Response Body -> " + JSON.stringify(responseData)
            )
        })
        .done();
  }
}
module.exports = api;
