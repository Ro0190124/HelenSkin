document.getElementById('searchToggle').addEventListener('click', function() {
    var searchBox = document.querySelector('.tk-trangchu');
    if (searchBox.style.display === 'none' || searchBox.style.display === '') {
        searchBox.style.display = 'block';
    } else {
        searchBox.style.display = 'none';
    }
});