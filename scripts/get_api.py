import os
import re
from collections import defaultdict

def derive_love2d_api(class_name, method_name):
    """
    Attempts to derive a Love2D-style API call.
    Example: Filesystem, GetInfo -> love.filesystem.getInfo
    """
    if not class_name or not method_name:
        return ""

    module_name = class_name.lower()

    # Convert PascalCase or camelCase method_name to camelCase (starting lowercase)
    # If it's already camelCase (like 'getInfo'), it should remain as is.
    # If it's PascalCase (like 'GetInfo'), it becomes 'getInfo'.
    love_method_name = method_name[0].lower() + method_name[1:]

    return f"love.{module_name}.{love_method_name}"

def parse_cs_file(filepath):
    """
    Parses a C# file to extract public static classes and their public static methods.
    """
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
    except Exception as e:
        print(f"Error reading file {filepath}: {e}")
        return None

    class_data = {}

    # Regex to find public static classes
    class_match = re.search(r"public\s+static\s+class\s+(\w+)", content)

    if class_match:
        class_name = class_match.group(1)
        class_content_start = class_match.end()

        # Rough way to get class content, assuming reasonably formatted code
        # This might fail with complex nested structures or preprocessor directives
        open_braces = 0
        class_body_end = class_content_start
        found_first_brace = False

        for i in range(class_content_start, len(content)):
            if content[i] == '{':
                if not found_first_brace:
                    found_first_brace = True
                open_braces += 1
            elif content[i] == '}':
                open_braces -= 1
                if found_first_brace and open_braces == 0:
                    class_body_end = i
                    break

        class_body = content[class_match.end():class_body_end]

        methods = defaultdict(list)
        # Regex to find public static methods, including parameters
        # This regex aims to capture [return_type] MethodName([params])
        # Group 1: return_type (non-greedy)
        # Group 2: method_name
        # Group 3: params_str
        method_pattern = re.compile(
            r"public\s+static\s+(?:async\s+)?(.*?)\s+(\w+)\s*\(([^)]*)\)"
        )

        for method_match in method_pattern.finditer(class_body):
            # Return type is group 1, method_name is group 2, params_str is group 3
            method_name = method_match.group(2)
            params_str = method_match.group(3).strip()

            # Clean up parameter string: remove default values, extra spaces
            params_list = []
            if params_str:
                raw_params = params_str.split(',')
                for p in raw_params:
                    p_cleaned = p.strip()
                    # Remove default initializers like "= null" or "= 12"
                    p_cleaned = re.sub(r"\s*=\s*.*", "", p_cleaned)
                    params_list.append(p_cleaned)

            full_signature = f"{method_name}({', '.join(params_list)})"
            methods[method_name].append(full_signature)

        if methods:
            class_data[class_name] = dict(methods)

    return class_data

def generate_markdown(all_class_data, output_file):
    """
    Generates a markdown file from the parsed API data.
    """
    markdown_lines = []

    markdown_lines.append(f"# Night / Love2D API\n")

    sorted_class_names = sorted(all_class_data.keys())

    for class_name in sorted_class_names:
        markdown_lines.append(f"## {class_name.lower()}\n")
        methods = all_class_data[class_name]
        sorted_method_names = sorted(methods.keys())

        for method_name in sorted_method_names:
            signatures = methods[method_name]
            love2d_call = derive_love2d_api(class_name, method_name)

            # Main function entry
            if love2d_call:
                markdown_lines.append(f"- {method_name}() - {love2d_call}")
            else:
                markdown_lines.append(f"- {method_name}()")

            # List overloads if more than one signature or if the single signature has params
            if len(signatures) > 1 or (len(signatures) == 1 and signatures[0] != f"{method_name}()"):
                for sig in sorted(signatures): # Sort signatures for consistent output
                    # Indented list for overloads
                    markdown_lines.append(f"  - {sig}")
        markdown_lines.append("") # Add a blank line between modules

    try:
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write("\n".join(markdown_lines))
        print(f"Markdown documentation generated at {output_file}")
    except Exception as e:
        print(f"Error writing markdown file {output_file}: {e}")

def main():
    framework_dir = os.path.join("src", "Night.Engine", "Framework")
    output_md_file = os.path.join("docs", "API.md")

    all_parsed_data = {}

    if not os.path.isdir(framework_dir):
        print(f"Error: Directory not found - {framework_dir}")
        return

    for root, _, files in os.walk(framework_dir):
        for filename in files:
            if filename.endswith(".cs"):
                filepath = os.path.join(root, filename)
                print(f"Parsing {filepath}...")
                parsed_data = parse_cs_file(filepath)
                if parsed_data:
                    for class_name, methods in parsed_data.items():
                        if class_name not in all_parsed_data:
                            all_parsed_data[class_name] = {}
                        # Merge methods, important if a class is split into partial classes (though not expected here)
                        for method_name, signatures in methods.items():
                            if method_name not in all_parsed_data[class_name]:
                                all_parsed_data[class_name][method_name] = []
                            all_parsed_data[class_name][method_name].extend(signatures)
                            # Ensure unique signatures
                            all_parsed_data[class_name][method_name] = sorted(list(set(all_parsed_data[class_name][method_name])))


    if all_parsed_data:
        # Ensure output directory exists
        os.makedirs(os.path.dirname(output_md_file), exist_ok=True)
        generate_markdown(all_parsed_data, output_md_file)
    else:
        print("No API data parsed.")

if __name__ == "__main__":
    main()
