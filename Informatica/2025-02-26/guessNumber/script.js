const dadi = ["&#9856;", "&#9857;", "&#9858;", "&#9859;", "&#9860;", "&#9861;"];

window.addEventListener('DOMContentLoaded', () => {
    const displayer = document.getElementById('dadi');
    const saldo = document.getElementById('saldo');
    const scommessa = document.getElementById('scommessa');
    const puntata = document.getElementById('puntata');

    document.getElementById('button').addEventListener('click', () => {
        let scommessaValue = parseInt(scommessa.value);
        let puntataValue = parseInt(puntata.value);

        if(typeof(puntataValue) != 'number' || isNaN(puntataValue) || typeof(scommessaValue) != 'number' || isNaN(scommessaValue)) {
            alert("Nella scommessa o nella puntata non sono stati inseriti numeri, riprova.");
            return;
        }

        if(puntataValue > parseInt(saldo.innerText)) {
            alert("Non hai abbastanza soldi per puntare questa cifra.");
            return;
        }

        if(puntataValue <= 0){
            alert("La puntata deve essere maggiore di 0.");
            return;
        }

        let value1 = Math.floor(Math.random()*6);
        let value2 = Math.floor(Math.random()*6);
        let sum = value1 + value2 + 2;

        if(sum == scommessaValue) 
            saldo.innerText = parseInt(saldo.innerText) + puntataValue;
        else 
            saldo.innerText = parseInt(saldo.innerText) - puntataValue;

        displayer.innerHTML = dadi[value1] + " " + dadi[value2] + " " + sum;
    });
})