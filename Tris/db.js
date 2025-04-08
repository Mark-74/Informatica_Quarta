const mariadb = require('mariadb');
const crypto = require('crypto');
const dotenv = require('dotenv');
dotenv.config();

const pool = mariadb.createPool({
    host: process.env.DB_HOST,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME,
    connectionLimit: 10
});

async function getConnection(){
    try {
        const conn = await pool.getConnection();
        return conn;
    } catch (err) {
        console.error(err);
        throw err;
    }
}

function login(){

}

function register(){

}

function getPlayerStats(){

}

module.exports = { login, register, getPlayerStats }