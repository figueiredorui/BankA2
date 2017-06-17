import { OnInit, OnDestroy, Directive, Input, ElementRef } from '@angular/core';
declare var $: any;

@Directive({
    selector: '[sparkline]'
})
export class SparklineDirective implements OnInit, OnDestroy {

    @Input() sparklineOptions;
     @Input('sparklineValues') sparklineValues;
     

    // generate a unique resize event so we can detach later
    public resizeEventId = 'resize.sparkline' + 1324;
    public $element;

    constructor(public el: ElementRef) {
        this.$element = $(el.nativeElement);
    }

    ngOnInit() {
        this.initSparkLine();
    }

    initSparkLine() {
        let options = this.sparklineOptions,
            data = this.$element.data();

        if (!options) {// if no scope options, try with data attributes
            options = data;
        }
        else {
            if (data) {// data attributes overrides scope options
                options = $.extend({}, options, data);
            }
        }

        options.type = options.type || 'bar'; // default chart is bar
        options.disableHiddenCheck = true;

        if (this.sparklineValues)
            this.$element.sparkline(this.sparklineValues, options);
        else
            this.$element.sparkline('html', options);

        if (options.resize) {
            $(window).on(this.resizeEventId, () => {
                if (this.sparklineValues)
                    this.$element.sparkline(this.sparklineValues, options);
                else
                    this.$element.sparkline('html', options);
            });
        }
    }

    ngOnDestroy() {
        $(window).off(this.resizeEventId);
    }
}
