// API базовый URL
const API_BASE_URL = 'http://localhost:5000/api';

// Загрузка отчетов
async function loadReports() {
    try {
        const response = await fetch(`${API_BASE_URL}/Reports`);
        const reports = await response.json();
        const reportsList = document.getElementById('reportsList');
        
        reportsList.innerHTML = reports.map(report => `
            <div class="umbrella-card">
                <h3 class="umbrella-card__title">${report.name}</h3>
                <p class="umbrella-card__description">${report.description || 'Нет описания'}</p>
                <div class="umbrella-card__actions">
                    <button class="umbrella-button umbrella-button--primary" onclick="generateReport('${report.id}')">
                        Сгенерировать
                    </button>
                </div>
            </div>
        `).join('');
    } catch (error) {
        console.error('Ошибка загрузки отчетов:', error);
    }
}

// Загрузка запланированных отчетов
async function loadScheduledReports() {
    try {
        const response = await fetch(`${API_BASE_URL}/ScheduledReports`);
        const scheduled = await response.json();
        const scheduledList = document.getElementById('scheduledList');
        
        scheduledList.innerHTML = scheduled.map(item => `
            <div class="umbrella-card">
                <h3 class="umbrella-card__title">${item.name}</h3>
                <p class="umbrella-card__description">Расписание: ${item.schedule}</p>
                <p class="umbrella-card__description">Отчет: ${item.reportName}</p>
                <div class="umbrella-card__actions">
                    <button class="umbrella-button umbrella-button--secondary" onclick="editScheduled('${item.id}')">
                        Редактировать
                    </button>
                </div>
            </div>
        `).join('');
    } catch (error) {
        console.error('Ошибка загрузки запланированных отчетов:', error);
    }
}

// Загрузка источников данных
async function loadDataSources() {
    try {
        const response = await fetch(`${API_BASE_URL}/DataSources`);
        const dataSources = await response.json();
        const dataSourcesList = document.getElementById('datasourcesList');
        
        dataSourcesList.innerHTML = dataSources.map(ds => `
            <div class="umbrella-card">
                <h3 class="umbrella-card__title">${ds.name}</h3>
                <p class="umbrella-card__description">Тип: ${ds.type}</p>
                <p class="umbrella-card__description">${ds.description || 'Нет описания'}</p>
                <div class="umbrella-card__actions">
                    <button class="umbrella-button umbrella-button--secondary" onclick="editDataSource('${ds.id}')">
                        Редактировать
                    </button>
                </div>
            </div>
        `).join('');
    } catch (error) {
        console.error('Ошибка загрузки источников данных:', error);
    }
}

// Генерация отчета
async function generateReport(reportId) {
    try {
        const response = await fetch(`${API_BASE_URL}/Reports/${reportId}/generate`, {
            method: 'POST'
        });
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `report-${reportId}.pdf`;
        a.click();
    } catch (error) {
        console.error('Ошибка генерации отчета:', error);
        alert('Ошибка генерации отчета');
    }
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
    loadReports();
    loadScheduledReports();
    loadDataSources();
});

