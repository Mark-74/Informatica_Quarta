

window.addEventListener('DOMContentLoaded', () => {
    console.log('caricato')
    displayer = document.getElementById('dadi');

    document.getElementById('button').addEventListener('click', () => {
        displayer.innerHTML = "&#9856; &#9857; &#9858; &#9859; &#9860; &#9861;"
    });
})