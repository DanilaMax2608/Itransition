document.addEventListener('DOMContentLoaded', function () {
    const errorRateSlider = document.getElementById('errorRateSlider');
    const errorRateInput = document.getElementById('errorRateInput');
    const seedInput = document.getElementById('seed');
    const randomSeedButton = document.getElementById('randomSeed');
    const dataTable = document.getElementById('dataTable').querySelector('tbody');
    const tableContainer = document.getElementById('tableContainer'); 
    let currentPage = 1;
    let loading = false; 

    errorRateSlider.addEventListener('input', syncErrorRate);
    errorRateInput.addEventListener('input', validateErrorRate);
    seedInput.addEventListener('input', validateSeedInput);
    randomSeedButton.addEventListener('click', setRandomSeed);
    tableContainer.addEventListener('scroll', loadMoreData); 

    function syncErrorRate() {
        const value = Math.min(1000, errorRateSlider.value);
        errorRateInput.value = value;
        clearTableAndGenerateData();
    }

    function validateErrorRate() {
        let value = parseInt(errorRateInput.value);
        if (value > 1000) {
            errorRateInput.value = 1000; 
        } else if (isNaN(value) || value < 0) {
            errorRateInput.value = ''; 
        }
        errorRateSlider.value = errorRateInput.value; 
        clearTableAndGenerateData(); 
    }

    function validateSeedInput() {
        const seed = parseInt(seedInput.value);
        if (seed < 0) {
            seedInput.value = ''; 
        } else if (!isNaN(seed)) {
            clearTableAndGenerateData();
        }
    }

    function setRandomSeed() {
        const randomSeed = Math.floor(Math.random() * 1000000);
        seedInput.value = randomSeed;
        clearTableAndGenerateData();
    }

    function clearTableAndGenerateData() {
        currentPage = 1; 

        const existingRows = Array.from(dataTable.querySelectorAll('tr'));
        const rowsToKeep = existingRows.slice(0, 20); 

        dataTable.innerHTML = '';

        rowsToKeep.forEach(row => {
            dataTable.appendChild(row);
        });

        tableContainer.scrollTop = 0;

        generateData();
    }

    function generateData() {
        const region = document.getElementById('region').value;
        const errorRate = parseInt(errorRateInput.value) || 0;
        const seed = parseInt(seedInput.value);

        fetch('/Home/GenerateData', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ region, errorRate, seed, page: currentPage })
        })
            .then(response => response.json())
            .then(data => {
                if (currentPage === 1) {
                    dataTable.innerHTML = '';
                }

                data.forEach(item => {
                    let existingRow = document.getElementById(`row-${item.id}`);
                    if (existingRow) {
                        existingRow.innerHTML = `
                        <td>${item.id}</td>
                        <td>${item.id}</td>
                        <td>${item.fullName}</td>
                        <td>${item.address}</td>
                        <td>${item.phoneNumber}</td>
                    `;
                    } else {
                        const row = document.createElement('tr');
                        row.id = `row-${item.id}`;
                        row.innerHTML = `
                        <td>${item.id}</td>
                        <td>${item.id}</td>
                        <td>${item.fullName}</td>
                        <td>${item.address}</td>
                        <td>${item.phoneNumber}</td>
                    `;
                        dataTable.appendChild(row);
                    }
                });

                loading = false; 
            })
            .catch(error => console.error('Error:', error));
    }

    function loadMoreData() {
        if (loading) return;
        const { scrollTop, scrollHeight, clientHeight } = tableContainer;

        if (scrollTop + clientHeight >= scrollHeight - 10) {
            loading = true;
            currentPage++;
            generateData(); 
        }
    }

    generateData(); 
});
