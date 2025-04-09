const express = require('express');
const jwt = require('jsonwebtoken');
const cookieParser = require('cookie-parser');
const path = require('path');
const dotenv = require('dotenv');
dotenv.config();

const crypto = require('crypto');
const { login, register, getPlayerStats } = require('./db.js');

const app = express();
const KEY = process.env.SECRET_KEY || crypto.randomBytes(32).toString('hex');

//setup ejs for templates and templates folder
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'templates'));

//settings for express
app.use(cookieParser());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use('/bootstrap', express.static(path.join(__dirname, 'node_modules/bootstrap/dist')));

app.post('/api/login', async (req, res) => {

});

app.post('/api/register', async (req, res) => {

});

app.get('/', async (req, res) => {
    
});

app.get('/scoreboard', async (req, res) => {

})

app.get('/player/:id', async (req, res) => {

})

app.get('/tris', async (req, res) => {

});

app.post('/tris', async (req, res) => {

})

const PORT = 3000;
app.listen(PORT, () => {
    console.log(`Server running at http://localhost:${PORT}`);
});