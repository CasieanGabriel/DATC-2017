var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');
var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var db = require('./db');
var index = require('./routes/index');
var bcrypt = require('bcrypt');
var sql = require('mssql');
var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');

// uncomment after placing your favicon in /public
//app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use('/', index);

sql.connect(db, function(err) {
  // create Request object
  var request = new sql.Request();

  // query to the database and get the records
  if (err) console.log(err)

  app.post('/tables', function (req, res) {
    // let username = req.body.username;
    // console.log(username);
     var reqString = "SELECT * FROM INFORMATION_SCHEMA.TABLES;";
    //  var reqString = "insert into transporturi(ID_Transport, Data, Status) values(1, '2017-06-06', 'ok')";
      request.query(reqString, function (err, recordset) {
        console.log(recordset.recordset);
          if (err) return res.send({
              error: err,
              message: "An error ocured"
          });
          return res.json({
              list: recordset,
              message: "Succes retrun"
          });
      });
  });

  app.post('/auth', function(req, res) {
    console.log(req.body);

    let Username = "'"+ req.body.username + "'";
    let Password =req.body.password;
    var reqString = "SELECT * FROM Users where Email="+Username+";";
    request.query(reqString, function(err, recordset) {
      console.log(recordset);
      if (err) return res.json({
        error: err,
        message: "An error ocured"
      });
      if(recordset.recordset){
        let password = recordset.recordset[0].Password;
        console.log(password);
        console.log(Password);
        let hash = bcrypt.compareSync(password, Password); // true
        if (!hash)
          return res.json({
            list: recordset.recordset[0],
            message: "ok"
          });

          return res.json({
            message: "nok"
          });
        }
      });

  });

  app.post('/create-account', function(req, res) {

    let email = "'" + req.body.email + "'";
    let password = "'" + req.body.password + "'";
    let confirm = "'" + req.body.confirmPassword + "'";
    console.log(password);
    console.log(confirm);
    let hash = bcrypt.compareSync(password, confirm); // true
    console.log(!hash);
    if (!hash){
      var reqString = "INSERT INTO Users (Email, Password, ConfirmPassword) VALUES(" + email + ', ' + password + ', ' + confirm + ')';
      console.log("INSERT INTO Users (Email, Password, confirmPassword) VALUES(" + email + ', ' + password + ', ' + confirm + ')');
      request.query(reqString, function(err, recordset) {
        //id_user = recordset.recordset[0].Id;
        console.log(email);
        if (err) return res.json({
          error: err,
          message: "An error ocured"
        });
        // send records as a response
        return res.json({
          message: "Succes insert"
        });
      });
    }
    else{
      return res.json({
          message: "Confirm password and password don't match!"
        });
    }
  });
  ///add_commands

  app.post('/add-resource', async function(req, res) {
    var error, message ={};
    var Temperature;
    var Humidity;
    var Flag, Longitudine, Latitudine;
console.log(req.body);
    for (var i = req.body.length - 1; i >= 0; i--) {
      Temperature =req.body[i].Temperature;
      Humidity =req.body[i].Humidity;
      Flag = req.body[i].Flag;
      Longitudine = req.body[i].Longitudine;
      Latitudine = req.body[i].Latitudine;
      var reqString = "select * from Resources WHERE Latitudine="+Latitudine+" AND Longitudine="+Longitudine;
      console.log(reqString);
      await request.query(reqString, function (err, recordset) {
          if( recordset.recordset.length === 0){
            console.log("aici");
            var reqString1 = "INSERT INTO Resources (Temperatura, Umiditatea, Latitudine, Longitudine, Flag) VALUES(" + Temperature + ', ' + Humidity + ', ' +Latitudine+ ', ' + Longitudine + ', ' + Flag + ')';
            console.log(reqString1);
            request.query(reqString1, function(err1, recordset1) {
              if (err1)
                error ='err';
              else
              message = recordset1;
            });
          }
          else{
             var reqString2 = "UPDATE Resources SET Temperatura="+ Temperature+ " Umiditatea=" + Humidity + " Flag=" + Flag + " WHERE Longitudine=" + Longitudine + " AND Latitudine=" + Latitudine;
            console.log(reqString2);
             request.query(reqString2, function(err2, recordset2) {
              if (err2)
                error ='err';
              else
              message = recordset2;
           });
          }
      });

    };
    return res.json({
              resources: message,
              message: "Succes return"
          });
  });
  ///add_commands

  app.get('/get-resource', function (req, res) {
    // let Longitudine =  req.query.Longitudine;
    // let Latitudine = req.query.Latitudine;
    var reqString = "select * from Resources";
     // var reqString = "select * from Resources r inner join Response res on r.Id = res.Id_Resources";
     // var reqString = "select * from Resources WHERE Latitudine="+Latitudine+" AND Longitudine="+Longitudine;
     // console.log("select * from Resources WHERE Latitudine="+Latitudine+" AND Longitudine="+Longitudine);
      request.query(reqString, function (err, recordset) {
          if (err) return res.json({
              error: err,
              message: "An error ocured"
          });
          return res.json({
              resources: recordset.recordset,
              message: "Succes return"
          });
      });
  });
  app.post('/add_response', function(req, res) {
    let Id_Resources = req.body.Id_Resources;
    let Color_temperature = "'" + req.body.Color_temperature + "'";
    let Color_humidity = "'" + req.body.Color_humidity + "'";
    var reqString = "INSERT INTO Response (Id_Resources, Culoare_temperatura, Culoare_umiditate) VALUES(" + Id_Resources + ', ' + Color_temperature + ', ' + Color_humidity + ')';
    console.log("da");
    request.query(reqString, function(err, recordset) {
      if (err) return res.json({
        error: err,
        message: "An error ocured"
      });
      // send records as a response
      return res.json({
        message: "Succes insert"
      });
    });
  });


  app.get('/get-response', function (req, res) {
      let Longitudine = req.query.Longitudine;
      let Latitudine = req.query.Latitudine;
      var reqString = "select * from Response r inner join Resources res on r.Id_resources=res.Id WHERE res.Latitudine="+Latitudine+" AND res.Longitudine="+Longitudine;
      console.log(reqString);
      request.query(reqString, function (err, recordset) {
          if (err) return res.json({
              error: err,
              message: "An error ocured"
          });
          return res.json({
              resources: recordset.recordset,
              message: "Succes retrun"
          });
      });
  });


});

module.exports = app;

