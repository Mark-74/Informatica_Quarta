// Marco Balducci 4H 28/03/2025
// Validazione form tramite jquery
$(document).ready(function() {
    // Metodi di validazione custom
    // Validazione solo lettere (inclusi caratteri Unicode, spazi, apostrofi e trattini)
    $.validator.addMethod("lettersOnly", function(value, element) {
        return this.optional(element) || /^[\p{L}\s'-]+$/u.test(value);
    }, "Solo lettere, spazi, apostrofi e trattini sono permessi");

    // Validazione formato username (8-64 caratteri, alfanumerici + underscore/trattini)
    $.validator.addMethod("usernamePattern", function(value, element) {
        return this.optional(element) || /^[a-zA-Z0-9_\-]{8,64}$/.test(value);
    }, "8-64 caratteri con lettere, numeri, underscore o trattini");

    // Validazione complessità password (min 8 caratteri, maiuscole, minuscole, numeri e simboli)
    $.validator.addMethod("passwordComplexity", function(value, element) {
        return this.optional(element) || 
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#_])[A-Za-z\d@$!%*?&#_]{8,}$/.test(value);
    }, "Minimo 8 caratteri con maiuscole, minuscole, numeri e simboli (@$!%*?&#_)");

    // Validazione data di nascita (tra il 1900 e 10 anni fa)
    $.validator.addMethod("validDOB", function(value, element) {
        const today = new Date();
        const birthDate = new Date(value);
        const minDate = new Date('1900-01-01');
        const maxDate = new Date(today.getFullYear() - 10, 11, 31); // 10 anni fa
        return birthDate >= minDate && birthDate <= maxDate;
    }, "La data deve essere compresa tra il 1900 e 10 anni fa");

    // Inizializzazione validazione modulo
    $("#form").validate({
        rules: {
            username: {
                required: true,
                usernamePattern: true
            },
            password: {
                required: true,
                passwordComplexity: true
            },
            email: {
                required: true,
                email: true
            },
            name: {
                required: true,
                lettersOnly: true,
                minlength: 2,
                maxlength: 50
            },
            surname: {
                required: true,
                lettersOnly: true,
                minlength: 2,
                maxlength: 50
            },
            datebirth: {
                required: true,
                validDOB: true
            }
        },
        // Messaggi di errore personalizzati
        messages: {
            username: { required: "Il nome utente è obbligatorio" },
            password: { required: "La password è obbligatoria" },
            email: { 
                required: "L'email è obbligatoria",
                email: "Formato email non valido"
            },
            name: { required: "Il nome è obbligatorio" },
            surname: { required: "Il cognome è obbligatorio" },
            datebirth: { required: "La data di nascita è obbligatoria" }
        },
        // Stile e posizionamento degli errori
        errorPlacement: function(error, element) {
            error.addClass('text-danger small');
            error.insertAfter(element);
        },
        // Gestore invio modulo
        submitHandler: function(form) {
            form.submit();
            alert("Modulo inviato con successo!");
        }
    });
});
