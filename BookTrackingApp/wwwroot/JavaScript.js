const sidebar = document.getElementById('sidebar');
const mediaQuery = window.matchMedia('(max-width: 414px)');


function handleMediaQuery(event) {
    if (event.matches) {  
        
        console.log('390');
    } else {
        sidebar.removeAttribute('hidden');
        console.log('768');
    }
}

/*handleMediaQuery(mediaQuery);*/

