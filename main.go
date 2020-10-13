package main

import (
	"bytes"
	"fmt"
	"log"
	"os"
	"os/exec"
	"strings"

	"github.com/tidwall/gjson"
)

func main() {
	log.SetFlags(0)

	var envPaths []envPath
	var cmdArgs []string
	for i := 1; i < len(os.Args); i++ {
		if strings.EqualFold(os.Args[i], "--env") {
			envPaths = append(envPaths, makeEnvPath(os.Args[i+1]))
			i++
		} else {
			cmdArgs = append(cmdArgs, os.Args[i])
		}
	}

	output, err := callCommand(cmdArgs)
	if err != nil {
		log.Fatalf("error calling command: %v", err)
	}

	if err = extractAndSet(output, envPaths); err != nil {
		log.Fatalf("error during extract and set: %v", err)
	}
}

func callCommand(args []string) ([]byte, error) {
	cmd := exec.Command(args[0], args[1:]...)
	stdout, err := cmd.StdoutPipe()
	if err != nil {
		return nil, fmt.Errorf("piping to stdout: %w", err)
	}

	if err = cmd.Start(); err != nil {
		return nil, fmt.Errorf("running command: %w", err)
	}

	buf := &bytes.Buffer{}
	if _, err = buf.ReadFrom(stdout); err != nil {
		return nil, fmt.Errorf("reading from command stdout: %w", err)
	}

	return buf.Bytes(), nil
}

func extractAndSet(output []byte, eps []envPath) error {
	for _, ep := range eps {
		value := gjson.GetBytes(output, ep.path)
		valueStr := value.String()
		if valueStr == "" {
			return fmt.Errorf("missing value: %q", ep.path)
		}
		if err := os.Setenv(ep.env, value.String()); err != nil {
			return fmt.Errorf("setting env var: %w", err)
		}

		log.Printf("set env var: %s", ep.env)
	}

	return nil
}

type envPath struct {
	path string
	env  string
}

func makeEnvPath(input string) envPath {
	parts := strings.Split(input, "~")

	return envPath{
		path: parts[0],
		env:  parts[1],
	}
}
