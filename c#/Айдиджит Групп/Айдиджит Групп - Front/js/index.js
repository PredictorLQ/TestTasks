class Roster {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
        this.offset = 0;
        this.count = 10;
    }

    static initial(baseUrl) {
        const _roster = new Roster(baseUrl);

        $(document).ready(function () {
            $(".roster-search").bind('input', () => { _roster.#loadPage(1); });

            $('body').on('click', '[data-page]', function () {
                let page = _roster.#getPage($(this));

                if (page <= 0 || this.page == page)
                    return;

                _roster.#loadPage(page);
            });

        });

        _roster.#loadPage(1);
    }

    #getPage(el) {
        let page = isNaN(parseInt(el.attr('data-page'))) ? 0 : parseInt(el.attr('data-page'));

        if (page > this.pagination.countPages)
            page = 0;

        return page;
    }

    #loadPage(page) {
        if (this.loading)
            return;

        this.loading = true;

        this.offset = this.count * (page - 1);
        this.page = page;

        $.get(`${this.baseUrl}/api/roster`, {
            offset: this.offset,
            count: this.count,
            searchText: $(".roster-search").val().trim()
        })
            .done((response) => this.#createContext(response))
            .fail(() => { })
            .always(() => { this.loading = false });
    }

    #createContext(response) {

        let html = this.#createContextItems(response.items)
            + this.#createContextPagination(response.pagination);

        $(".roster-results").html(html);
    }

    #createContextItems(items) {
        function createItem(item) {
            return `<div class="card mb-3">
                        <div class="card-header">${item.title}</div>
                        <div class="card-body">
                            <blockquote class="blockquote mb-0">
                                <p>${item.subTitle}</p>
                            </blockquote>
                        </div>
                    </div>`;
        }

        return items.reduce((str, item) => str + createItem(item), '');
    }

    #createContextPagination(pagination) {
        this.pagination = pagination;

        if (pagination.countPages < 2)
            return '';

        let html = '';

        if (pagination.page > 1)
            html += `<li class="page-item">
                        <span class="page-link pointer" aria-label="Previous" data-page="${(pagination.page - 1)}">
                            <span aria-hidden="true">&laquo;</span>
                        </span>
                    </li>`;


        for (let i = 1; i <= pagination.countPages; i++)
            html += `<li class="page-item ${i == pagination.page ? 'active' : ''}">
                        <span class="page-link pointer" data-page="${i}">${i}</span>
                    </li>`;

        if (pagination.page < pagination.countPages)
            html += `<li class="page-item">
                        <span class="page-link pointer" aria-label="Next" data-page="${(pagination.page + 1)}">
                            <span aria-hidden="true">&raquo;</span>
                        </span>
                    </li>`;

        return `<nav aria-label="Page navigation example mt-4">
                    <ul class="pagination">
                       ${html}
                    </ul>
                </nav>`;
    }
}

Roster.initial('https://localhost:7115');