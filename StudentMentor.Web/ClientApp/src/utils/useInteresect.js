import { useState, useRef, useEffect } from "react";

const useIntersect = ({ root = null, rootMargin, threshold = 0 }) => {
  const [entry, updateEntry] = useState({});
  const [node, setNode] = useState(null);

  const observer = useRef(null);

  useEffect(() => {
    if (observer.current) {
      observer.current.disconnect();
    }

    observer.current = new IntersectionObserver(
      ([entry]) => {
        updateEntry(entry);
      },
      {
        root,
        rootMargin,
        threshold,
      }
    );

    if (node) {
      observer.current.observe(node);
    }

    return () => observer.current.disconnect();
  }, [node, root, rootMargin, threshold]);

  const unSubscribe = () => {
    if (observer.current) {
      observer.current.disconnect();
    }
  };

  return [setNode, entry, unSubscribe];
};

export default useIntersect;
