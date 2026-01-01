document.addEventListener('DOMContentLoaded', function () {
    const parallaxScene = document.getElementById('parallaxScene');

    // 3D Parallax Mouse Tracking (Kept because it's cool and doesn't affect brightness)
    if (parallaxScene) {
        const layers = parallaxScene.querySelectorAll('.parallax-layer');
        document.addEventListener('mousemove', function (e) {
            const mouseX = e.clientX / window.innerWidth;
            const mouseY = e.clientY / window.innerHeight;
            layers.forEach(layer => {
                const depth = layer.getAttribute('data-depth') || 0.5;
                const moveX = (mouseX - 0.5) * depth * 50;
                const moveY = (mouseY - 0.5) * depth * 50;
                layer.style.transform = `translateX(${moveX}px) translateY(${moveY}px) rotateY(${(mouseX - 0.5) * depth * 20}deg) rotateX(${-(mouseY - 0.5) * depth * 20}deg)`;
            });
        });
    }

    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href !== '#' && href.length > 1) {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            }
        });
    });

    // Animated counter for stats (Kept this as it only affects numbers)
    const animateValue = (element, start, end, duration) => {
        let startTimestamp = null;
        const step = (timestamp) => {
            if (!startTimestamp) startTimestamp = timestamp;
            const progress = Math.min((timestamp - startTimestamp) / duration, 1);
            element.textContent = typeof end === 'number' ? Math.floor(progress * (end - start) + start) : end;
            if (progress < 1) window.requestAnimationFrame(step);
        };
        window.requestAnimationFrame(step);
    };

    const statsObserver = new IntersectionObserver(function (entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const statValue = entry.target;
                const finalValue = statValue.textContent;
                if (finalValue !== '∞') {
                    const numValue = parseInt(finalValue);
                    if (!isNaN(numValue)) animateValue(statValue, 0, numValue, 1500);
                }
                statsObserver.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    document.querySelectorAll('.stat-value').forEach(stat => statsObserver.observe(stat));
});

// ALL SCROLL BRIGHTENING / HEADER CHANGING LOGIC REMOVED
