var express = require('express');

var app = express();


app.use('/static', express.static(__dirname + '/public'));

app.set('views',__dirname + '/views');
app.set('view engine', 'ejs');

var data={
    "date": "23321",
    "temperature": "3213213",
    "humidity"   : "654654",
    "pressure"   : "6546546"
}


app.get('/', function(req, res) {
    return res.render('index',{data});
});

app.listen(process.env.PORT || 3000, function(){
    console.log("App is live! localhost:3000");
});