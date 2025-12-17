const sidebar = document.getElementById('sidebar');
const menuBtn = document.getElementById('menu_btn');

    menuBtn.addEventListener('click', () => {
        sidebar.classList.toggle('minimize');
    });