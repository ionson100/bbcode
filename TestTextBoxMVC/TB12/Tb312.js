
$(function () {
    $('#scr-panel').left = $('#src').left;
     yok();
});

function yok() {
    $.beautyOfCode.init('clipboard.swf', {
        noGutter: false //если true то показывать номера строк не сможет
    }
    );
    $.beautyOfCode.beautifyAll();
}

function showpanel(t) {
    
    $('#scr-panel').css('display') == "none" ?$('#scr-panel').css('display', 'block') : $('#scr-panel').css('display', 'none');
    $('#scr-panel').offset({ top: document.getElementById('src').offsetTop+25, left: document.getElementById('src').offsetLeft });

}

function panelClorShow() {
    $('#color-panel').css('display') == "none" ? $('#color-panel').css('display', 'block') : $('#color-panel').css('display', 'none');
    $('#color-panel').offset({ top: document.getElementById('fcolor').offsetTop + 25, left: document.getElementById('fcolor').offsetLeft });
}

function insertCode(codes, codee) {
    $('#scr-panel').css('display', 'none');
    $('#color-panel').css('display', 'none');
    document.getElementById('tb-core').focus();
    if (document.getElementById('tb-core').selectionStart ||
        document.getElementById('tb-core').selectionStart == '0') {
            var selStart = document.getElementById('tb-core').selectionStart;
            var selEnd = document.getElementById('tb-core').selectionEnd;
            var s = document.getElementById('tb-core').value;
            s = s.substring(0, selStart) + codes + s.substring(selStart, selEnd) +
                codee + s.substring(selEnd, s.length);
            document.getElementById('tb-core').value = s;
            if (selEnd != selStart) {
                document.getElementById('tb-core').setSelectionRange(selStart, selEnd +
                    codes.length + codee.length);
            } else {
                document.getElementById('tb-core').setSelectionRange(selStart +
                    codes.length, selStart + codes.length);
            }
        }
        else {
            document.getElementById('tb-core').value += codes + codee;
        }
}

function preliminaryshow() { //показ превью
    $('.modal-body').load('/tb/Tb312/Index/o', { data: escape($('#tb-core').val()) }, function () {
        $('#modal-tb').css('display', 'block');
        yok();
    });
}
