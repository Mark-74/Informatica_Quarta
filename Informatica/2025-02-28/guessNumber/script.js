// Marco Balducci 4H 28/02/2025
// Array contenente i simboli Unicode per i sei lati di un dado
const dadi = ["&#9856;", "&#9857;", "&#9858;", "&#9859;", "&#9860;", "&#9861;"];

// Quando il contenuto del DOM è stato caricato, esegue la funzione
window.addEventListener('DOMContentLoaded', () => {
    // Seleziona gli elementi HTML tramite i loro ID
    const displayer = document.getElementById('dadi');
    const saldo = document.getElementById('saldo');
    const scommessa = document.getElementById('scommessa');
    const puntata = document.getElementById('puntata');

    // Aggiunge un listener di eventi al pulsante con ID 'button'
    document.getElementById('button').addEventListener('click', () => {
        // Converte i valori delle scommesse e delle puntate in interi
        let scommessaValue = parseInt(scommessa.value);
        let puntataValue = parseInt(puntata.value);

        // Controlla se i valori di 'puntata' e 'scommessa' sono numeri validi
        if(typeof(puntataValue) != 'number' || isNaN(puntataValue) || typeof(scommessaValue) != 'number' || isNaN(scommessaValue)) {
            alert("Nella scommessa o nella puntata non sono stati inseriti numeri, riprova.");
            return;
        }

        // Controlla se la puntata è maggiore del saldo disponibile
        if(puntataValue > parseInt(saldo.innerText)) {
            alert("Non hai abbastanza soldi per puntare questa cifra.");
            return;
        }

        // Controlla se la puntata è inferiore o uguale a zero
        if(puntataValue <= 0){
            alert("La puntata deve essere maggiore di 0.");
            return;
        }

        // Genera due numeri casuali tra 0 e 5 (inclusi), corrispondenti ai lati del dado
        let value1 = Math.floor(Math.random() * 6);
        let value2 = Math.floor(Math.random() * 6);
        // Calcola la somma dei due valori dei dadi e incrementa di 2 per ottenere un risultato tra 2 e 12
        let sum = value1 + value2 + 2;

        // Aggiorna il saldo in base al risultato della scommessa
        if(sum == scommessaValue) 
            saldo.innerText = parseInt(saldo.innerText) + puntataValue;
        else 
            saldo.innerText = parseInt(saldo.innerText) - puntataValue;

        // Visualizza i simboli dei dadi e la somma nell'elemento 'dadi'
        displayer.innerHTML = dadi[value1] + " " + dadi[value2] + " " + sum;
    });
})
