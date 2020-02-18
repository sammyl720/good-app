var gulp = require('gulp');
var uglify = require('gulp-uglify');
var minifycss = require('gulp-minify-css');
var imagemin = require('gulp-imagemin');
var cache = require('gulp-cache');
var del = require('del');
var useref = require('gulp-useref');
var gulpif = require('gulp-if');
var jshint = require('gulp-jshint');

gulp.task('images', function() {
    return gulp.src('app/images/**/*')
        .pipe(cache(imagemin({
            optimizationLevel: 3,
            progressive: true,
            interlaced: true
        })))
        .pipe(gulp.dest('dist/images'));
});

gulp.task('html', function() {
    var assets = useref.assets();
    return gulp.src('app/index.html')
        .pipe(assets)
        .pipe(gulpif('*.js', uglify()))
        .pipe(gulpif('*.css', minifycss()))
        .pipe(assets.restore())
        .pipe(useref())
        .pipe(gulp.dest('dist'));
});

gulp.task('views', function() {
    return gulp.src('app/views/**/*.html')
        .pipe(gulp.dest('dist/views/'));
});

gulp.task('clean', function(callback) {
    del(['dist'], callback);
});

gulp.task('fonts', function() {
    return gulp.src(['app/bower_components/bootstrap/fonts/*.*', 'app/bower_components/fontawesome/fonts/*.*'])
        .pipe(gulp.dest('dist/fonts'));
});

gulp.task('webconfig', function() {
    return gulp.src('app/web.config')
        .pipe(gulp.dest('dist'));
});

gulp.task('default', ['clean'], function() {
    gulp.start('images', 'html', 'views', 'fonts', 'webconfig');
});

gulp.task('lint', function() {
    return gulp.src('app/scripts/**/*.js')
        .pipe(jshint())
        .pipe(jshint.reporter('default'));
});

gulp.task('watch', function() {
    gulp.watch('app/scripts/**/*.js', ['lint']);
});
